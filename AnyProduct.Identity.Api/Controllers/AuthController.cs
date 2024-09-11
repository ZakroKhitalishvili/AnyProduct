using AnyProduct.Identity.Api.Data;
using AnyProduct.Identity.Api.Dtos;
using AnyProduct.Identity.Api.Entities;
using AnyProduct.Identity.Api.Services;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace AnyProduct.Identity.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public partial class AuthController(
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager,
    ITokenService tokenService,
    IdentityContext context,
    IConfiguration configuration) : ControllerBase
{


    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var user = new User
        {
            FullName = registerDto.Username,
            Email = registerDto.Email,
            UserName = registerDto.Email,
            PersonalNumber = registerDto.PersonalNumber,


        };

        var result = await userManager.CreateAsync(user, registerDto.Password);

        await userManager.AddToRoleAsync(user, "User");

        if (result.Succeeded)
        {

            return Ok();
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {

        if (loginDto is null)
        {
            return BadRequest("Provide credentials");
        }

        var user = await userManager.FindByEmailAsync(loginDto.Email);

        if (user is null)
            return Unauthorized();

        if (!await userManager.CheckPasswordAsync(user, loginDto.Password))
            return Unauthorized();

        var roles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email!),
            new Claim(ClaimTypes.GivenName, user.FullName),
            new Claim("Role", string.Join(",",roles)),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id)
        };
        var accessToken = tokenService.GenerateAccessToken(claims);
        var refreshToken = tokenService.GenerateRefreshToken();

        var refreshTokenEntry = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Active = true,
            Device = Request.Headers["User-Agent"],
            Expiration = DateTime.UtcNow.AddDays(10),
            Token = refreshToken,
            UserId = user.Id,
        };

        context.RefreshTokens.Add(refreshTokenEntry);

        context.SaveChanges();

        return Ok(new AuthenticatedResponse
        {
            Token = accessToken,
            RefreshToken = refreshToken
        });

    }

    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenDto refreshTokenDto)
    {
        if (refreshTokenDto is null)
            return BadRequest("Invalid client request");

        string accessToken = refreshTokenDto.Token;
        string refreshToken = refreshTokenDto.RefreshToken;
        var principal = tokenService.GetPrincipalFromExpiredToken(accessToken);
        var username = principal.Identity.Name; //this is mapped to the Name claim by default
        var user = await userManager.FindByEmailAsync(username);
        var roles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email!),
            new Claim(ClaimTypes.GivenName, user.FullName),
            new Claim("Role", string.Join(",",roles)),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id)
        };

        var refreshTokenEntry = context.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);

        if (user is null || refreshTokenEntry is null || !refreshTokenEntry.Active || refreshTokenEntry.Expiration <= DateTime.Now)
            return BadRequest("Invalid client request");

        var newAccessToken = tokenService.GenerateAccessToken(claims);


        context.SaveChanges();
        return Ok(new AuthenticatedResponse()
        {
            Token = newAccessToken,
            RefreshToken = refreshToken,
        });
    }

    [HttpPost]
    [Route("revoke")]
    public async Task<IActionResult> Revoke(RevokeDto revokeDto)
    {
        var username = User.Identity.Name;
        var user = await userManager.FindByEmailAsync(username);
        var refreshTokenEntry = context.RefreshTokens.SingleOrDefault(x => x.Token == revokeDto.RefreshToken);

        if (user is null || refreshTokenEntry is null) return BadRequest();

        refreshTokenEntry.Active = false;
        context.Entry(refreshTokenEntry).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

        context.SaveChanges();

        return NoContent();
    }
}

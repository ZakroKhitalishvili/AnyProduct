using AnyProduct.Orders.Application.Services;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace AnyProduct.Orders.Api.Services;

public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    public string UserId  {
        get {
            var userIdClaim = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier);
            if(userIdClaim is null)
            {
                throw new Exception("There no claim provided for User Id");
            }

            return userIdClaim.Value;
        }
        }
    private IHttpContextAccessor _httpContextAccessor { get; } = httpContextAccessor;
}

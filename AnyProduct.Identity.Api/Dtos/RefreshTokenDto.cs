using System.ComponentModel.DataAnnotations;

namespace AnyProduct.Identity.Api.Dtos;

public class RefreshTokenDto
{
    [Required]
    public string Token {  get; set; }

    [Required]
    public string RefreshToken { get; set; }

}

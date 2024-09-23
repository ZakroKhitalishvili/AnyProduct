using System.ComponentModel.DataAnnotations;

namespace AnyProduct.Identity.Api.Dtos;

public class RegisterDto
{
    [Required, EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string MobileNumber { get; set; }

    [Required]
    public required string PersonalNumber { get; set; }

    [Required]
    public required string Username { get; set; }

    [Required]
    public required string Password { get; set; }

}

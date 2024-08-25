using System.ComponentModel.DataAnnotations;

namespace AnyProduct.Identity.Api.Dtos;

public class RegisterDto
{
    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string MobileNumber { get; set; }

    [Required]
    public string PersonalNumber { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

}

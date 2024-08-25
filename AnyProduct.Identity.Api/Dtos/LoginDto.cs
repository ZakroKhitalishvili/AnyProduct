using System.ComponentModel.DataAnnotations;

namespace AnyProduct.Identity.Api.Dtos;

public class LoginDto
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

}

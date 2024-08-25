using System.ComponentModel.DataAnnotations;

namespace AnyProduct.Identity.Api.Controllers;


public class RevokeDto
{
    [Required]
    public string RefreshToken { get; set; }
}


using System.ComponentModel.DataAnnotations;

namespace AnyProduct.Identity.Api.Entities;

public class RefreshToken
{
    [Key]
    public Guid Id { get; set; }

    public string UserId { get; set; }

    public string Token { get; set; }

    public DateTime Expiration { get; set; }

    public string Device { get; set; }

    public bool Active { get; set; }

}

using System.ComponentModel.DataAnnotations;

namespace AnyProduct.Orders.Domain.Entities.Basket;

public class BasketItemDto
{

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public int Units { get; set; }

}

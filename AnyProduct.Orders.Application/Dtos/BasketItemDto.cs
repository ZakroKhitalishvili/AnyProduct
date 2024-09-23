using System.ComponentModel.DataAnnotations;

namespace AnyProduct.Orders.Application.Dtos.Basket;

public class BasketItemDto
{

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public int Units { get; set; }

}

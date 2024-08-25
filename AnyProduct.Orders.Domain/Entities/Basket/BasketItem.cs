using System.ComponentModel.DataAnnotations;

namespace AnyProduct.Orders.Domain.Entities.Basket;

public class BasketItem
{
    public Guid Id { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public int Units { get; set; }

    public DateTime CreatedDate { get; set; }
}

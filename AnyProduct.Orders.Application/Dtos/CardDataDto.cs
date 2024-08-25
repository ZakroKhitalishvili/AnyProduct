

using AnyProduct.Orders.Domain.Entities.Buyer;

namespace AnyProduct.Orders.Application.Dtos;

public class CardDataDto
{
    public string CardNumber { get; set; }

    public string CardHolderName { get; set; }

    public DateTime CardExpiration { get; set; }

    public string CardSecurityNumber { get; set; }

    public CardType CardType { get; set; }

}

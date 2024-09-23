using System.ComponentModel.DataAnnotations;

namespace AnyProduct.Orders.Domain.Entities.BuyerAggregate;

public class PaymentMethod
{
    public Guid Id { get; set; }

    [Required]
    public string Alias { get; private set; }

    [Required]
    public string CardNumber { get; private set; }

    public string SecurityNumber { get; private set; }

    [Required]
    public string CardHolderName { get; private set; }

    public DateTime Expiration { get; private set; }

    public CardType CardType { get; private set; }

    protected PaymentMethod() { }

    public PaymentMethod(CardType cardType, string alias, string cardNumber, string securityNumber, string cardHolderName, DateTime expiration)
    {
        CardNumber = !string.IsNullOrWhiteSpace(cardNumber) ? cardNumber : throw new ArgumentException("Card number is not provided", nameof(cardNumber));
        SecurityNumber = !string.IsNullOrWhiteSpace(securityNumber) ? securityNumber : throw new ArgumentException("Security number is not provided", nameof(securityNumber));
        CardHolderName = !string.IsNullOrWhiteSpace(cardHolderName) ? cardHolderName : throw new ArgumentException("Card holder name is not provided", nameof(cardHolderName));

        if (expiration < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Card is expired");
        }

        Alias = alias;
        Expiration = expiration;
        CardType = cardType;
        Id = Guid.NewGuid();
    }

    public bool IsEqualTo(CardType cardtype, string cardNumber, DateTime expiration)
    {
        return CardType == cardtype
            && CardNumber == cardNumber
            && Expiration == expiration;
    }

    public string GetBriefDescription()
    {
        return $"{CardType} {CardHolderName} {CardNumber.Substring(0, 4)} **** **** ****";
    }
}

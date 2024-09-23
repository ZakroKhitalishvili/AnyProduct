
namespace AnyProduct.Orders.Domain.Entities.OrderAggregate;

public record AddressValueObject
(
     string Street,
     string City,
     string State,
     string Country,
     string ZipCode
);

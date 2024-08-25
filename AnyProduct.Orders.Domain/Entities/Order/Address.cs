
namespace AnyProduct.Orders.Domain.Entities.Order;

public record AddressValueObject
(
     string Street,
     string City,
     string State,
     string Country,
     string ZipCode
);

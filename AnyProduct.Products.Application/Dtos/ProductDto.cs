
namespace AnyProduct.Products.Application.Dtos;

public class ProductDto
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public bool IsInStock { get; set; }

    public int UnitsInStock { get; set; }

    public decimal Price { get; set; }

    public required string ImagePath { get; set; }

    public required IReadOnlyCollection<ProductCategoryDto> ProductCategories { get; set; }

}

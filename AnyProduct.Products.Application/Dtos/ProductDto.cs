
namespace AnyProduct.Products.Application.Dtos;

public class ProductDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public bool IsInStock { get; set; }

    public int UnitsInStock { get; set; }

    public decimal Price { get; set; }

    public string ImagePath { get; set; }

    public ICollection<ProductCategoryDto> ProductCategories{ get; set; }

}


using System.ComponentModel.DataAnnotations;

namespace AnyProduct.Products.Domain.Entities;

public class ProductCategory : AggregateRoot
{

    [Required, StringLength(50)]
    public string Name { get; private set; }

    public DateTime CreateDate { get; private set; }

    public DateTime? UpdateDate { get; private set; }

    protected ProductCategory() { }

    public ProductCategory(string name, DateTime createDate)
    {
        Name = name;
        CreateDate = createDate;
        AggregateId = Guid.NewGuid();
    }

    public void Update(string name, DateTime updateDate)
    {
        Name = name;
        UpdateDate = updateDate;
    }

}



using AnyProduct.Products.Domain.Entities;

namespace AnyProduct.Products.Domain.Repositories;

public interface IProductCategoryRepository
{
    ProductCategory Add(ProductCategory product);
    void Update(ProductCategory product);
    void Delete(Guid id);
    ProductCategory FindById(Guid id);
    ICollection<ProductCategory> FindManyById(ICollection<Guid> id);
    ICollection<ProductCategory> GetList(out int totalSize, int page = 1, int pageSize = 10);
    bool ExistName(string name);
}

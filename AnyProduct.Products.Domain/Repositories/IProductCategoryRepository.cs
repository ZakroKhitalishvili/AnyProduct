

using AnyProduct.Products.Domain.Entities;

namespace AnyProduct.Products.Domain.Repositories;

public interface IProductCategoryRepository
{
    ProductCategory Add(ProductCategory productCategory);
    void Update(ProductCategory productCategory);
    void Delete(Guid id);
    ProductCategory? FindById(Guid id);
    ICollection<ProductCategory> FindManyById(ICollection<Guid> ids);
    ICollection<ProductCategory> GetList(out int totalSize, int page = 1, int pageSize = 10);
    bool ExistName(string name);
}

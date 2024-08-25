

using AnyProduct.Products.Domain.Entities;
using System;

namespace AnyProduct.Products.Domain.Repositories;

public interface IProductRepository
{
    Product Add(Product product);
    void Update(Product product);
    void Delete(Guid id);
    Product FindById(Guid id);
    ICollection<Product> GetList(out int totalSize, ICollection<Guid> categories, int page = 1, int pageSize = 10);
}

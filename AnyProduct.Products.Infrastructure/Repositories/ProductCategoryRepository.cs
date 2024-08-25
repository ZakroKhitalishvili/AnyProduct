

using AnyProduct.Products.Domain.Entities;
using AnyProduct.Products.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AnyProduct.Products.Infrastructure.Repositories;

public class ProductCategoryRepository : IProductCategoryRepository
{
    private readonly ProductContext _context;

    public ProductCategoryRepository(ProductContext context)
    {
        _context = context;
    }

    public ProductCategory Add(ProductCategory productCategory)
    {
        return _context.ProductCategory.Add(productCategory).Entity;
    }

    public void Delete(Guid id)
    {
        _context.ProductCategory.Where(x => x.AggregateId == id).ExecuteDelete();
    }

    public bool ExistName(string name)
    {
        return _context.ProductCategory.Where(x => x.Name == name).Any();
    }

    public ProductCategory? FindById(Guid id)
    {
        return _context.ProductCategory.SingleOrDefault(x => x.AggregateId == id);
    }

    public ICollection<ProductCategory> FindManyById(ICollection<Guid> ids)
    {
        return _context.ProductCategory.Where(x => ids.Contains(x.AggregateId)).ToList();
    }

    public ICollection<ProductCategory> GetList(out int totalSize, int page = 1, int pageSize = 10)
    {
        var query = _context.ProductCategory.AsNoTracking();


        totalSize = query.Count();
        return query.OrderBy(x => x.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public void Update(ProductCategory productCategory)
    {
        _context.Entry(productCategory).State = EntityState.Modified;
    }
}

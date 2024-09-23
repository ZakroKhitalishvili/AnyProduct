

using AnyProduct.Products.Domain.Entities;
using AnyProduct.Products.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AnyProduct.Products.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductContext _context;

    public ProductRepository(ProductContext context)
    {
        _context = context;
    }

    public Product Add(Product product)
    {
        return _context.Products.Add(product).Entity;
    }

    public void Delete(Guid id)
    {
        _context.Products.Where(x => x.AggregateId == id).ExecuteDelete();
    }

    public Product? FindById(Guid id)
    {
        return _context.Products.SingleOrDefault(x => x.AggregateId == id);
    }

    public ICollection<Product> GetList(out int totalSize, ICollection<Guid>? categories, int page = 1, int pageSize = 10)
    {
        var query = _context.Products.AsNoTracking();


        if (categories is not null && categories.Count > 0)
        {
            query = query.Where(x => x.ProductCategoryIds.Any(id => categories.Contains(id)));
        }

        totalSize = query.Count();
        return query.OrderByDescending(x => x.CreatedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public void Update(Product product)
    {
        _context.Entry(product).State = EntityState.Modified;
    }
}

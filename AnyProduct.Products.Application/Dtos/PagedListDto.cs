
namespace AnyProduct.Products.Application.Dtos;

public class PagedListDto<T>
{
    public ICollection<T> Items { get; set; }

    public int Total { get; set; }

    public int Page { get; set; }

    public int PageSize { get; set; }
}

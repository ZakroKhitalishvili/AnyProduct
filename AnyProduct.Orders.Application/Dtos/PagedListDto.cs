namespace AnyProduct.Orders.Application.Dtos;

public class PagedListDto<T>
{
    public required ICollection<T> Items { get; init; }

    public int Total { get; set; }

    public int Page { get; set; }

    public int PageSize { get; set; }
}

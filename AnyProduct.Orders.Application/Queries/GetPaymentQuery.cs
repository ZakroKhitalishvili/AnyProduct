
using AnyProduct.Orders.Application.Dtos;
using AnyProduct.Orders.Domain.Entities.PaymentAggregate;
using AnyProduct.Orders.Domain.Repositories;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Orders.Application.Queries;

public class GetPaymentQuery : IRequest<PagedListDto<PaymentDto>>
{
    public int? Page { get; set; }

    public int? PageSize { get; set; }
}

public class GetPaymentQueryHandler : IRequestHandler<GetPaymentQuery, PagedListDto<PaymentDto>>
{
    private readonly IPaymentRepository _paymentRepository;

    public GetPaymentQueryHandler(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public Task<PagedListDto<PaymentDto>> Handle([NotNull] GetPaymentQuery request, CancellationToken cancellationToken)
    {
        request.Page ??= 1;
        request.PageSize ??= 10;

        var payments = _paymentRepository.GetList(out int totalSize, null, request.Page.Value, request.PageSize.Value);

        var result = new PagedListDto<PaymentDto>()
        {
            Items = new List<PaymentDto>(),
            Page = request.Page.Value,
            PageSize = request.PageSize.Value,
            Total = totalSize,
        };
        foreach (var payment in payments)
        {
            result.Items.Add(new PaymentDto
            {
                BuyerId = payment.BuyerId,
                ExternalPaymentRequestId = payment.ExternalPaymentRequestId,
                Orderid = payment.Orderid,
                PaymentDate = payment.PaymentDate,
                PaymentMethodDescription = payment.PaymentMethodDescription,
                Price = payment.Price,
            });
        }


        return Task.FromResult(result);
    }
}

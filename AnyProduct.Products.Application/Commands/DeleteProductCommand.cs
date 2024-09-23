using AnyProduct.Products.Application.Services;
using AnyProduct.Products.Domain.Repositories;
using AnyProduct.Products.Domain.Services;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Products.Application.Commands;

public class DeleteProductCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IProductRepository _productRepository;
    private readonly IFileService _fileService;

    public DeleteProductCommandHandler(IProductRepository productRepository, IFileService fileService, IDateTimeProvider dateTimeProvider)
    {
        _productRepository = productRepository;
        _fileService = fileService;
    }

    public async Task<Unit> Handle([NotNull] DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = _productRepository.FindById(request.Id);

        if (product is null)
            throw new InvalidOperationException("Product not found");


        if (product.Image is not null)
        {
            await _fileService.DeleteAsync(product.Image);
        }

        _productRepository.Delete(request.Id);

        return Unit.Value;
    }

}

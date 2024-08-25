using AnyProduct.Products.Application.Services;
using AnyProduct.Products.Domain.Repositories;
using AnyProduct.Products.Domain.Services;
using MediatR;

namespace AnyProduct.Products.Application.Commands;

public class DeleteProductCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IProductRepository _productRepository;
    private readonly IFileService _fileService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public DeleteProductCommandHandler(IProductRepository productRepository, IFileService fileService, IDateTimeProvider dateTimeProvider)
    {
        _productRepository = productRepository;
        _fileService = fileService;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = _productRepository.FindById(request.Id);

        if (product is null)
            throw new Exception("Product not found");

        string image = product.Image;

        if (product.Image is not null)
        {
            await _fileService.DeleteAsync(product.Image);
        }

        _productRepository.Delete(request.Id);

        return Unit.Value;
    }

}

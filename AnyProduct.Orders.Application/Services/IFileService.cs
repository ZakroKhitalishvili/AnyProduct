using Microsoft.AspNetCore.Http;

namespace AnyProduct.Orders.Application.Services;

public interface IFileService
{
    Task<(string originalName, string uniqueName)> UploadAsync(IFormFile file);

    Task<bool> DeleteAsync(string uniqueName);
}

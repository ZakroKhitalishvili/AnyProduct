

using Microsoft.AspNetCore.Http;

namespace AnyProduct.Products.Application.Services;

public interface IFileService
{
    Task<(string originalName, string uniqueName)> UploadAsync(IFormFile file);

    Task<bool> DeleteAsync(string uniqueName);
}

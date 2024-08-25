
using AnyProduct.Products.Application.Services;
using Microsoft.AspNetCore.Http;

namespace AnyProduct.Products.Infrastructure.Services;

public class LocalFileService : IFileService
{
    public const string RootFolder = "UploadedFiles";

    public Task<bool> DeleteAsync(string uniqueName)
    {
        try
        {
            var filePath = $"{RootFolder}/{uniqueName}";

            FileInfo fileInfo = new FileInfo(filePath);
            fileInfo.Delete();
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            return Task.FromResult(false);
        }
    }

    public async Task<(string originalName, string uniqueName)> UploadAsync(IFormFile formFile)
    {

        if (formFile.Length > 0)
        {
            string uniqueName = $"{Guid.NewGuid()}{Path.GetExtension(formFile.FileName)}";
            var filePath = $"{RootFolder}/{uniqueName}";

            Directory.CreateDirectory(RootFolder);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            return (formFile.Name, uniqueName);
        }

        throw new ArgumentException("File is empty", nameof(formFile));
    }
}

using AnyProduct.Orders.Application.Services;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;

namespace AnyProduct.Orders.Infrastructure.Services;

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
        catch (IOException)
        {
            return Task.FromResult(false);
        }
    }

    public async Task<(string originalName, string uniqueName)> UploadAsync([NotNull] IFormFile file)
    {

        if (file.Length > 0)
        {
            string uniqueName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = $"{RootFolder}/{uniqueName}";

            Directory.CreateDirectory(RootFolder);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return (file.Name, uniqueName);
        }

        throw new ArgumentException("File is empty", nameof(file));
    }
}

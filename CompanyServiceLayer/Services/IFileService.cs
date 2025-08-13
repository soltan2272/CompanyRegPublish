using Microsoft.AspNetCore.Http;

namespace CompanyServiceLayer.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveImageAsync(IFormFile imageFile, string folderName);
        bool DeleteImage(string imagePath);
        string? GetImageUrl(string? imagePath);
    }
}

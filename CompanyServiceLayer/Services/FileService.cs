using CompanyServiceLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CompanyServiceLayer.Services
{
    public class FileService : IFileService
    {
        private readonly IConfiguration _configuration;
        private readonly string _uploadPath;
        private readonly string _baseUrl;

        public FileService(IConfiguration configuration)
        {
            _configuration = configuration;
            _uploadPath = _configuration["FileStorage:UploadPath"] ?? "wwwroot/images";
            _baseUrl = _configuration["FileStorage:BaseUrl"] ?? "/images";
            
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        public async Task<string> SaveImageAsync(IFormFile imageFile, string folderName)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                    throw new ArgumentException("No file provided");

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(fileExtension))
                    throw new ArgumentException("Invalid file type. " +
                        "Only image files are allowed.");

                var folderPath = Path.Combine(_uploadPath, folderName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return Path.Combine(folderName, fileName).Replace("\\", "/");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to save image: {ex.Message}");
            }
        }

        public bool DeleteImage(string imagePath)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath))
                    return false;

                var fullPath = Path.Combine(_uploadPath, imagePath);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public string? GetImageUrl(string? imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return null;

            return $"{_baseUrl}/{imagePath}";
        }
    }
}

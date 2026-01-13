using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using zizo_shop.Application.Common.Interfaces;

namespace zizo_shop.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {
            // Define the path: wwwroot/uploads/products
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", folderName);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Generate unique name
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return the web-friendly path
            return $"/uploads/{folderName}/{fileName}";
        }

        public void DeleteFile(string filePath)
        {
            // Logic to remove file from disk when a product is deleted
            var fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
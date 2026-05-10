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
            using var stream = file.OpenReadStream();
            return await UploadFileAsync(stream, file.FileName, folderName);
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string folderName)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", folderName);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(stream);
            }

            return $"/uploads/{folderName}/{uniqueFileName}";
        }

        public void DeleteFile(string filePath)
        {
            var fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/'));
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}

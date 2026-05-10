namespace zizo_shop.Application.Common.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderName);
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string folderName);
        void DeleteFile(string filePath);
    }
}

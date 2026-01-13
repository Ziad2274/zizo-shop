namespace zizo_shop.Application.Common.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile fileName, string folderName);
        void DeleteFile(string filePath);
    }
}

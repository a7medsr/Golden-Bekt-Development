using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FX.Services.Bunny
{
    public interface IBunyimagesServices
    {
        Task<(bool Success, string FilePath, string FileName, string ImageUrl, string ErrorMessage)> UploadFileAsync(IFormFile image, string FolderName);
        public Task<byte[]> DownloadImage(string FolderName, string fileName);
        public Task<string> GetImageUrl(string FolderName, string fileName);
        public string GenerateNewFilename(string originalFilename);
    }
}

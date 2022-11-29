using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NovEShop.Share.Helpers
{
    public interface IFileStorageHelper
    {
        Task<string> SaveFile(IFormFile file);
        string GetFileUrl(string fileName);
        Task SaveFileAsync(Stream mediaBinaryStream, string fileName);
        Task DeleteFileAsync(string fileName);
        string GetFileName(string filePath);
        string GenerateFilePath(string fileName);
    }

    public class FileStorageHelper : IFileStorageHelper
    {
        private readonly string _userContentFolder;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";

        public FileStorageHelper(IWebHostEnvironment host)
        {
            _userContentFolder = Path.Combine(host.ContentRootPath, USER_CONTENT_FOLDER_NAME);
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }

        public string GetFileUrl(string fileName)
        {
            return $"{USER_CONTENT_FOLDER_NAME}/{fileName}";
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(fileStream);
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await SaveFileAsync(file.OpenReadStream(), fileName);

            return fileName;
        }

        public string GetFileName(string filePath)
        {
            return filePath.Substring(_userContentFolder.Length);
        }

        public string GenerateFilePath(string fileName)
        {
            return $"{_userContentFolder}/{fileName}";
        }
    }
}

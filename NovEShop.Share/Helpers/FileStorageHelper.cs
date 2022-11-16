using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NovEShop.Share.Helpers
{
    public interface IFileStorageHelper
    {
        string GetFileUrl(string fileName);
        Task SaveFileAsync(Stream mediaBinaryStream, string fileName);
        Task DeleteFileAsync(string fileName);
    }

    public class FileStorageHelper : IFileStorageHelper
    {
        private readonly string _userContentFolder;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";

        public FileStorageHelper(IWebHostEnvironment host)
        {

        }

        public Task DeleteFileAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public string GetFileUrl(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task SaveFileAsync(Stream mediaBinaryStream, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}

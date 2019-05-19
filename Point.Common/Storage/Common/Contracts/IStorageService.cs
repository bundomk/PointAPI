using System.IO;
using System.Threading.Tasks;

namespace Point.Common.Storage.Common.Contracts
{
    public interface IStorageService
    {
        IStorageService SetRootStoragePath(string rootStoragePath);

        Task UploadAsync(Stream flleStream, string fileName, string contentType = null, bool allowOverwrite = false);
        Task UploadTextAsync(string text, string fileName, string contentType = null, bool allowOverwrite = false);
        Task UploadToDirectoryAsync(Stream fileStream, string directoryPath, string fileName, string contentType = null, bool allowOverwrite = false);

        Task<Stream> DownloadAsync(string fileName, bool suppressExceptions = false);
        Task<string> DownloadTextAsync(string fileName, bool suppressExceptions = false);
        Task<Stream> DownloadFromDirectoryAsync(string directoryPath, string fileName, bool suppressExceptions = false);

        Task RemoveAsync(string fileName);
    }
}

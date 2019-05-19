using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Point.Common.Storage.Common.Contracts;
using Point.Common.Storage.Common.Helpers;
using Point.Common.Storage.Common.Options;

namespace Point.Common.Storage.Common
{
    public class BlobStorageService : BaseAzureStorageService, IStorageService
    {
        private readonly object _workingContainerLock = new object();
        private string _containerName;
        private string _containerDirectoryPath;

        private CloudBlobClient _cloudBlobClient;
        private CloudBlobContainer _workingContainer;

        protected CloudBlobClient CloudBlobClient
        {
            get
            {
                if (_cloudBlobClient == null)
                {
                    _cloudBlobClient = CloudStorageAccount.CreateCloudBlobClient();
                }

                return _cloudBlobClient;
            }
        }

        protected CloudBlobContainer WorkingContainer
        {
            get
            {
                if (_workingContainer == null)
                {
                    lock (_workingContainerLock)
                    {
                        if (_workingContainer != null)
                            return _workingContainer;

                        var containerReference = CloudBlobClient.GetContainerReference(_containerName);
                        
                        _workingContainer = containerReference;
                    }
                }

                return _workingContainer;
            }
        }

        public BlobStorageService(IOptions<AzureStorageOptions> storageOptions) 
            : base(storageOptions)
        { }

        public async Task UploadAsync(Stream flleStream, string fileName, string contentType = null, bool allowOverwrite = false)
        {
            var fullFileName = GetFullFileName(fileName);

            await CreateContainerIfNotExistsAsync();

            var blockBlob = WorkingContainer.GetBlockBlobReference(fullFileName.RemoveUTF8ControlCharacters());

            var blobExists = await blockBlob.ExistsAsync();

            if (!allowOverwrite && blobExists)
            {
                return;
            }
            
            if (!String.IsNullOrEmpty(contentType))
            {
                blockBlob.Properties.ContentType = contentType;
            }

            await blockBlob.UploadFromStreamAsync(flleStream);
        }

        public async Task UploadTextAsync(string text, string fileName, string contentType = null, bool allowOverwrite = false)
        {
            var fullFileName = GetFullFileName(fileName);

            await CreateContainerIfNotExistsAsync();

            var blockBlob = WorkingContainer.GetBlockBlobReference(fullFileName.RemoveUTF8ControlCharacters());

            var blobExists = await blockBlob.ExistsAsync();

            if (!allowOverwrite && blobExists)
            {
                return;
            }
            
            if (!String.IsNullOrEmpty(contentType))
            {
                blockBlob.Properties.ContentType = contentType;
            }

            await blockBlob.UploadTextAsync(text);
        }

        public async Task UploadToDirectoryAsync(Stream fileStream, string directoryPath, string fileName, string contentType = null, bool allowOverwrite = false)
        {
            var fullFileName = fileName;

            if (!String.IsNullOrEmpty(directoryPath))
            {
                fullFileName = $"{directoryPath}/{fileName}";
            }

            await UploadAsync(fileStream, fullFileName, contentType, allowOverwrite);
        }

        public async Task<Stream> DownloadAsync(string fileName, bool suppressExceptions = false)
        {
            var fullFileName = GetFullFileName(fileName);

            var blockBlob = WorkingContainer.GetBlockBlobReference(fullFileName.RemoveUTF8ControlCharacters());

            var blobExists = await blockBlob.ExistsAsync();

            if (!blobExists)
            {
                if (suppressExceptions)
                {
                    return null;
                }

                throw new Exception($"The requested file {fileName} does not exists!");
            }

            var fileStream = new MemoryStream();

            await blockBlob.DownloadToStreamAsync(fileStream);

            return fileStream;
        }

        public async Task<string> DownloadTextAsync(string fileName, bool suppressExceptions = false)
        {
            var fullFileName = GetFullFileName(fileName);

            var blockBlob = WorkingContainer.GetBlockBlobReference(fullFileName.RemoveUTF8ControlCharacters());

            var blobExists = await blockBlob.ExistsAsync();

            if (!blobExists)
            {
                if(suppressExceptions)
                {
                    return null;
                }

                throw new Exception($"The requested file {fileName} does not exists!");
            }

            var fileStream = new MemoryStream();

            var text = await blockBlob.DownloadTextAsync();

            return text;
        }

        public async Task<Stream> DownloadFromDirectoryAsync(string directoryPath, string fileName, bool suppressExceptions = false)
        {
            return await DownloadAsync($"{directoryPath}/{fileName}", suppressExceptions);
        }

        public async Task RemoveAsync(string fileName)
        {
            var fullFileName = GetFullFileName(fileName);

            var blockBlob = WorkingContainer.GetBlockBlobReference(fullFileName.RemoveUTF8ControlCharacters());

            var blobExists = await blockBlob.ExistsAsync();

            if (!blobExists)
                return;

            await blockBlob.DeleteAsync();
        }

        public IStorageService SetRootStoragePath(string rootStoragePath)
        {
            var parameters = GetRootStoragePathParts(rootStoragePath);

            var containerName = parameters.First().ToString();
            var containerDirectoryPath = parameters.Count() > 1 ? String.Join("/", parameters.Where(x => !x.Equals(containerName, StringComparison.OrdinalIgnoreCase))) : default(String);

            _containerName = containerName;
            _containerDirectoryPath = containerDirectoryPath;

            ResetWorkingContainer();

            return this;
        }

        private void ResetWorkingContainer()
        {
            lock (_workingContainerLock)
            {
                _workingContainer = null;
            }
        }

        private string GetFullFileName(string fileName)
        {
            var fullFileName = fileName;

            if (!String.IsNullOrEmpty(_containerDirectoryPath))
            {
                fullFileName = $"{_containerDirectoryPath}/{fileName}";
            }

            return fullFileName;
        }

        private async Task CreateContainerIfNotExistsAsync()
        {
            await WorkingContainer.CreateIfNotExistsAsync();
        }
    }
}

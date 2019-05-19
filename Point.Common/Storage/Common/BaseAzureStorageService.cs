using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using System;
using System.Collections.Generic;
using Point.Common.Storage.Common.Options;

namespace Point.Common.Storage.Common
{
    public class BaseAzureStorageService
    {
        private readonly string _storageUsername;
        private readonly string _storagePassword;

        private CloudStorageAccount _cloudStorageAccount;

        protected BaseAzureStorageService(IOptions<AzureStorageOptions> storageOptions)
        {
            _storageUsername = storageOptions.Value.Username;
            _storagePassword = storageOptions.Value.Password;
        }

        protected CloudStorageAccount CloudStorageAccount
        {
            get
            {
                if (_cloudStorageAccount == null)
                {
                    _cloudStorageAccount = new CloudStorageAccount(new StorageCredentials(_storageUsername, _storagePassword), false);
                }

                return _cloudStorageAccount;
            }
        }

        protected IEnumerable<string> GetRootStoragePathParts(string rootStoragePath)
        {
            if (String.IsNullOrEmpty(rootStoragePath))
                throw new Exception("[BaseAzureStorageManager]: rootStoragePath cannot be NULL or EMPTY!!");

            var parameters = rootStoragePath.Split('/');

            if (parameters.Length < 1)
                throw new Exception("[BaseAzureStorageManager]: rootStoragePath must contains at least 1 part!");

            return parameters;
        }
    }
}

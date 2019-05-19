using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using Point.Common.Storage.Common.Contracts;
using Point.Common.Extensions;

namespace Point.Common.Storage.Core
{
    public class CacheObjectService<T> : ICacheObjectService<T>
    {
        private readonly IStorageService _storageService;

        public StringBuilder StringHash { get; set; }
        public string Id { get; set; }
        public T DataContent { get; set; }
        public string CacheKey { get; set; }

        public CacheObjectService(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public ICacheObjectService<T> SetDataContent(T dataContent)
        {
            DataContent = dataContent;

            return this;
        }

        public ICacheObjectService<T> AddHashItem(string hashItem)
        {
            this.StringHash.Append(hashItem);

            return this;
        }

        public ICacheObjectService<T> SetRowKey(string key)
        {
            this.Id = key;

            return this;
        }

        public ICacheObjectService<T> ComputeRowKey()
        {
            var stringHash = StringHash.ToString();

            this.Id = stringHash.ToHashString();

            return this;
        }

        public void InsertEntity()
        {
            Task.Run(() => InsertEntityAsync());
        }

        public async Task InsertEntityAsync()
        {
            var text = JsonConvert.SerializeObject(this);

            CheckCacheKey();

            await _storageService
                    .SetRootStoragePath(CacheKey)
                    .UploadTextAsync(text.Compress(), Id, allowOverwrite: true);
        }

        public void RemoveEntity()
        {
            Task.Run(() => RemoveEntityAsync());
        }

        public async Task RemoveEntityAsync()
        {
            CheckCacheKey();

            await _storageService
                    .SetRootStoragePath(CacheKey)
                    .RemoveAsync(Id);
        }

        public ICacheObjectService<T> FindEntity()
        {
            FindEntityAsync().Wait();

            return this;
        }

        public async Task<ICacheObjectService<T>> FindEntityAsync()
        {
            CheckCacheKey();

            var resultText = await _storageService
                                        .SetRootStoragePath(CacheKey)
                                        .DownloadTextAsync(Id, suppressExceptions: true);

            PopulateResult(resultText);

            return this;
        }

        private void CheckCacheKey()
        {
            if (String.IsNullOrEmpty(CacheKey))
            {
                throw new Exception($"[CacheObjectService.CheckCacheKey]: CacheKey is not provided for key {nameof(T)}");
            }
        }

        private void PopulateResult(string resultText)
        {
            CacheObjectService<T> result = null;

            if (!String.IsNullOrEmpty(resultText))
            {
                string decompressResult = null;

                try
                {
                    decompressResult = resultText.Decompress();
                }
                catch (Exception)
                {
                    decompressResult = resultText;
                }

                if (result == null)
                {
                    result = JsonConvert.DeserializeObject<CacheObjectService<T>>(decompressResult);
                }
            }

            if(result == null)
            {
                throw new Exception($"Cache object with CacheKey = {CacheKey} and Id = {Id} cannot be found.");
            }

            DataContent = result.DataContent;
            Id = result.Id;
            StringHash = result.StringHash;
        }
    }
}

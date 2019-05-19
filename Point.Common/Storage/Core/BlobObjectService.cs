using System.Text;
using Point.Common.Storage.Common.Contracts;
using Point.Common.Storage.Core.Contracts;

namespace Point.Common.Storage.Core
{
    public class BlobObjectService<T, Z> : CacheObjectService<T>, IBlobObjectService<T, Z>
    {
        public BlobObjectService(IStorageService storageService)
            : base(storageService)
        {
            StringHash = new StringBuilder();
            CacheKey = typeof(Z).Name.ToLower();
        }
    }
}

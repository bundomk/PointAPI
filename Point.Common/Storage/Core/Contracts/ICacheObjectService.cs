using System.Text;
using System.Threading.Tasks;

namespace Point.Common.Storage.Core
{
    public interface ICacheObjectService<T>
    {
        StringBuilder StringHash { get; set; }
        string Id { get; set; }
        T DataContent { get; set; }
        string CacheKey { get; set; }

        ICacheObjectService<T> SetDataContent(T dataContent);
        ICacheObjectService<T> AddHashItem(string hashItem);
        ICacheObjectService<T> SetRowKey(string key);
        ICacheObjectService<T> ComputeRowKey();
        void InsertEntity();
        Task InsertEntityAsync();
        void RemoveEntity();
        Task RemoveEntityAsync();
        ICacheObjectService<T> FindEntity();
        Task<ICacheObjectService<T>> FindEntityAsync();
    }
}

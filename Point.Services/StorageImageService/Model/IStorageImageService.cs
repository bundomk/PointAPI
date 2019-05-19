using System.Threading.Tasks;

namespace Point.Services.ReturnImageService.Model
{
    public interface IStorageImageService
    {
        Task<byte[]> GetImageAsync(string url, string name);

        Task<string> PostImageAsync(byte[] bytes);
    }
}

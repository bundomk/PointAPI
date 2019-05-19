using Point.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Point.Services.ImageService.Model
{
    public interface IImageService
    {
        Dictionary<string, string> GetMetadata(byte[] byteArray);

        Task<Image> CreateImageAsync(byte[] byteArray, long userId);
    }
}

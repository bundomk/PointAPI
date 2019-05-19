using Point.Data.Models;
using Point.Data.Repository.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Point.Data.Repositories.Contracts
{
    public interface IImageRepository : IRepository<Image>
    {
        Task<List<Image>> GetImagesByInfoIdAsync(long id);
    }
}

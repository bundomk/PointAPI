using Point.Data.Models;
using Point.Data.Repository.Model;
using System.Threading.Tasks;

namespace Point.Data.Repositories.Contracts
{
    public interface IZonePointRepository : IRepository<ZonePoint>
    {
        Task RemoveAllAsync(long zoneId);
    }
}

using Point.Contracts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Point.Services.InstitutionService.Model
{
    public interface IInstitutionService
    {
        Task AddAsync(Institution institution);
        Task<Institution> GetAsync(long institutionId);
        Task<List<Institution>> GetAllAsync();
        Task AddZoneAsync(Zone zone);
        Task UpdateZonePointsAsync(long zoneId, List<ZonePoint> zonePoints);
        Task<List<Zone>> GetAllZonesAsync();
    }
}

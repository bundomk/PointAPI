using Point.Contracts.Models;
using Point.Data.Repositories.Contracts;
using Point.Services.InstitutionService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Point.Services.InstitutionService
{
    public class InstitutionService : IInstitutionService
    {
        private readonly IInstitutionRepository _institutionRepository;
        private readonly IZoneRepository _zoneRepository;
        private readonly IZonePointRepository _zonePointRepository;

        public InstitutionService(IInstitutionRepository institutionRepository, IZoneRepository zoneRepository, IZonePointRepository zonePointRepository)
        {
            _institutionRepository = institutionRepository;
            _zoneRepository = zoneRepository;
            _zonePointRepository = zonePointRepository;
        }

        public async Task AddAsync(Institution institution)
        {
            await _institutionRepository.AddAsync(new Data.Models.Institution()
            {
                Address = institution.Address,
                City = institution.City,
                Country = institution.Country,
                Description = institution.Description,
                Email = institution.Email,
                Latitude = Convert.ToDecimal(institution.Latitude),
                Longitude = Convert.ToDecimal(institution.Longitude),
                Name = institution.Name,
                Number = institution.Number,
                Phone = institution.Phone,
                ResponsiblePersonName = institution.ResponsiblePersonName
            });
        }

        public async Task AddZoneAsync(Zone zone)
        {
            await _zoneRepository.AddAsync(new Data.Models.Zone()
            {
                Description = zone.Description,
                Name = zone.Name,
                InstitutionId = zone.InstitutionId,
                ZonePoint = CreatePoints(zone.Points)
            });
        }

        public async Task<List<Institution>> GetAllAsync()
        {
            var insts = await _institutionRepository.GetAllAsync(false);
            List<Institution> result = new List<Institution>();

            foreach (var inst in insts)
            {
                result.Add(GetInstitution(inst));
            }

            return result;
        }

        public async Task<List<Zone>> GetAllZonesAsync()
        {
            var zones = await _zoneRepository.GetAllAsync(x => x.Id == x.Id, c => c.ZonePoint);
            List<Zone> result = new List<Zone>();

            foreach (var zone in zones)
            {
                result.Add(GetZone(zone));
            }

            return result;
        }

        public async Task<Institution> GetAsync(long institutionId)
        {
            var inst = await _institutionRepository.GetAsync(institutionId, false);

            return GetInstitution(inst);
        }

        public async Task UpdateZonePointsAsync(long zoneId, List<ZonePoint> zonePoints)
        {
            await _zonePointRepository.RemoveAllAsync(zoneId);

            await _zonePointRepository.AddRangeAsync(CreatePoints(zonePoints, zoneId));
        }

        private Institution GetInstitution(Data.Models.Institution institution)
        {
            return new Institution()
            {
                Id = institution.Id,
                Latitude = Convert.ToDouble(institution.Latitude),
                Longitude = Convert.ToDouble(institution.Longitude),
                Description = institution.Description,
                CreationTime = institution.CreationTime,
                Address = institution.Address,
                City = institution.City,
                Country = institution.Country,
                Number = institution.Number,
                Name = institution.Name,
                ResponsiblePersonName = institution.ResponsiblePersonName,
                Phone = institution.Phone,
                Email = institution.Email,
            };
        }

        private Zone GetZone(Data.Models.Zone zone)
        {
            return new Zone()
            {
                Id = zone.Id,
                Description = zone.Description,
                CreationTime = zone.CreationTime,
                Name = zone.Name,
                InstitutionId = zone.InstitutionId,
                Points = zone.ZonePoint.Select(x => new ZonePoint() { Latitude = Convert.ToDouble(x.Latitude), Longitude = Convert.ToDouble(x.Longitude) }).ToList()
            };
        }

        private List<Data.Models.ZonePoint> CreatePoints(List<ZonePoint> points, long zoneId = 0)
        {
            var result = points.Select(x => new Data.Models.ZonePoint()
            {
                ZoneId = zoneId,
                Latitude = Convert.ToDecimal(x.Latitude),
                Longitude = Convert.ToDecimal(x.Longitude)
            }).ToList();

            return result;
        }
    }
}

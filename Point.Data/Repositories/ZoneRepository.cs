using Point.Data.Models;
using Point.Data.Repositories.Contracts;
using Point.Data.Repository;

namespace Point.Data.Repositories
{
    public class ZoneRepository : Repository<Zone>, IZoneRepository
    {
        protected PointAdvisorContext Context => _context as PointAdvisorContext;

        public ZoneRepository(PointAdvisorContext context) : base(context)
        { }
    }
}

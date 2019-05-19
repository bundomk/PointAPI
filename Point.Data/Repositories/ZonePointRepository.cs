using System;
using System.Threading.Tasks;
using Point.Data.Models;
using Point.Data.Repositories.Contracts;
using Point.Data.Repository;
using System.Linq;

namespace Point.Data.Repositories
{
    public class ZonePointRepository : Repository<ZonePoint>, IZonePointRepository
    {
        protected PointAdvisorContext Context => _context as PointAdvisorContext;

        public ZonePointRepository(PointAdvisorContext context) : base(context)
        { }

        public async Task RemoveAllAsync(long zoneId)
        {
            var old = Context.ZonePoint.Where(x => x.ZoneId == zoneId);
            Context.RemoveRange(old);
            await Context.SaveChangesAsync();
        }
    }
}

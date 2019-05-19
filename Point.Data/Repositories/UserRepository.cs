using System;
using System.Threading.Tasks;
using Point.Data.Models;
using Point.Data.Repositories.Contracts;
using Point.Data.Repository;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Point.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        protected PointAdvisorContext Context => _context as PointAdvisorContext;

        public UserRepository(PointAdvisorContext context) : base(context)
        { }

        public async Task<User> GetUserByKeyAsync(Guid key)
        {
            return await Context.User.AsNoTracking().FirstOrDefaultAsync(x => x.Key.Equals(key));
        }

        public async Task<User> GetUserByDeviceIdAsync(string deviceId)
        {
            return await Context.User.AsNoTracking().FirstOrDefaultAsync(x => x.DeviceId.Equals(deviceId));
        }
    }
}

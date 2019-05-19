using Point.Data.Models;
using Point.Data.Repository.Model;
using System;
using System.Threading.Tasks;

namespace Point.Data.Repositories.Contracts
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByKeyAsync(Guid key);
        Task<User> GetUserByDeviceIdAsync(string deviceId);
    }
}

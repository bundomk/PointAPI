using Point.Contracts.Models;
using System;
using System.Threading.Tasks;

namespace Point.Services.RegisterService.Model
{
    public interface IRegisterService
    {
        Task<long> RegisterDeviceAsync(User user);
        Task<User> GetUserByIdAsync(long userId);
        Task<User> GetUserByKeyAsync(Guid key);
        Task<User> GetUserByDeviceIdAsync(string deviceId);
    }
}

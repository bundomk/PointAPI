using Point.Contracts.Models;
using Point.Data.Repositories.Contracts;
using Point.Services.RegisterService.Model;
using System;
using System.Threading.Tasks;

namespace Point.Services.RegisterService
{
    public class RegisterService : IRegisterService
    {
        private readonly IUserRepository _userRepository;

        public RegisterService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByIdAsync(long userId)
        {
            var model = await _userRepository.GetAsync(userId, false);

            if (model == null)
                return null;

            return new User()
            {
                Description = model.Description,
                DeviceId = model.DeviceId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Id = model.Id,
                Key = model.Key
            };
        }

        public async Task<User> GetUserByKeyAsync(Guid key)
        {
            var model = await _userRepository.GetUserByKeyAsync(key);

            if (model == null)
                return null;

            return new User()
            {
                Description = model.Description,
                DeviceId = model.DeviceId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Id = model.Id,
                Key = model.Key
            };
        }

        public async Task<User> GetUserByDeviceIdAsync(string deviceId)
        {
            var model = await _userRepository.GetUserByDeviceIdAsync(deviceId);

            if (model == null)
                return null;

            return new User()
            {
                Description = model.Description,
                DeviceId = model.DeviceId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Id = model.Id,
                Key = model.Key
            };
        }

        public async Task<long> RegisterDeviceAsync(User user)
        {
            var newUser = await _userRepository.AddAsync(new Data.Models.User()
            {
                Description = user.Description,
                DeviceId = user.DeviceId,
                FirstName = user.FirstName,
                LastName = user.LastName
            });

            return newUser.Id;
        }
    }
}
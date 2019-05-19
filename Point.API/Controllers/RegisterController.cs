using Microsoft.AspNetCore.Mvc;
using Point.Contracts.Models;
using Point.Services.RegisterService.Model;
using System;
using System.Threading.Tasks;

namespace Point.API.Controllers
{
    [Produces("application/json")]
    [Route("api/register")]
    public class RegisterController : Controller
    {
        private readonly IRegisterService _registerService;

        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost("user")]
        public async Task<IActionResult> UserAsync([FromBody] User user)
        {
            if (string.IsNullOrEmpty(user?.DeviceId))
            {
                return BadRequest("Invalid device id requested");
            }

            long key;
            var userDB = await _registerService.GetUserByDeviceIdAsync(user.DeviceId);

            if (userDB == null)
            {
                key = await _registerService.RegisterDeviceAsync(user);
            }
            else
            {
                key = userDB.Id;
            }

            return new ObjectResult(key);
        }
    }
}
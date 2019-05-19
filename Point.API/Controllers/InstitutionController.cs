using Microsoft.AspNetCore.Mvc;
using Point.Contracts.Models;
using Point.Services.InstitutionService.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Point.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Institution")]
    public class InstitutionController : Controller
    {
        private readonly IInstitutionService _institutionService;

        public InstitutionController(IInstitutionService institutionService)
        {
            _institutionService = institutionService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync([FromBody] Institution institution)
        {
            if (string.IsNullOrEmpty(institution?.Name))
            {
                return BadRequest("Invalid name requested");
            }

            await _institutionService.AddAsync(institution);

            return Ok();
        }

        [HttpPost("addzone")]
        public async Task<IActionResult> AddZoneAsync([FromBody] Zone zone)
        {
            if (string.IsNullOrEmpty(zone?.Name))
            {
                return BadRequest("Invalid name requested");
            }

            if (!zone.Points.Any())
            {
                return BadRequest("Zone points not provided");
            }

            await _institutionService.AddZoneAsync(zone);

            return Ok();
        }

        [HttpPost("updatepoints")]
        public async Task<IActionResult> UpdatePointsAsync([FromBody] ZonePointsUpdate zone)
        {
            if (zone.ZoneId == 0)
            {
                return BadRequest("Zone id not provided");
            }

            if (!zone.Points.Any())
            {
                return BadRequest("Zone points not provided");
            }

            await _institutionService.UpdateZonePointsAsync(zone.ZoneId, zone.Points);

            return Ok();
        }

        [HttpGet("get/{institutionId}")]
        public async Task<Institution> GetAsync(long institutionId)
        {
            return await _institutionService.GetAsync(institutionId);
        }

        [HttpGet("get")]
        public async Task<List<Institution>> GetAllAsync()
        {
            return await _institutionService.GetAllAsync();
        }
    }
}
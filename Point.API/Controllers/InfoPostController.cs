using Microsoft.AspNetCore.Mvc;
using Point.Contracts.Models;
using Point.Services.InfoPostService.Model;
using System.Threading.Tasks;

namespace Point.API.Controllers
{
    [Produces("application/json")]
    [Route("api/info")]
    public class InfoPostController : Controller
    {
        private readonly IInfoPostService _infoPostService;
        
        public InfoPostController(IInfoPostService infoPostService)
        {
            _infoPostService = infoPostService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync([FromBody] InfoPost info)
        {
            if (info?.UserId == null)
            {
                return BadRequest("Invalid user id requested");
            }

            await _infoPostService.AddAsync(info);

            return Ok();
        }

        [HttpGet("approve/{id}")]
        public async Task<IActionResult> ApproveAsync(long id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid post id requested");
            }

            var success = await _infoPostService.IsApprovedAsync(id, true);

            if (success)
            {
                return Ok();
            }

            return StatusCode(500);
        }

        [HttpGet("reject/{id}")]
        public async Task<IActionResult> RejectAsync(long id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid post id requested");
            }

            var success = await _infoPostService.IsApprovedAsync(id, false);

            if (success)
            {
                return Ok();
            }

            return StatusCode(500);
        }

        [HttpGet("like/{userid}/{postid}/{undo?}")]
        public async Task<IActionResult> LikeAsync(long userId, long postId, bool undo = false)
        {
            if (userId == 0)
            {
                return BadRequest("Invalid user id requested");
            }

            if (postId == 0)
            {
                return BadRequest("Invalid post id requested");
            }

            await _infoPostService.VoteAsync(userId, postId, true, undo);

            return Ok();
        }

        [HttpGet("dislike/{userid}/{postid}/{undo?}")]
        public async Task<IActionResult> DisLikeAsync(long userId, long postId, bool undo = false)
        {
            if (userId == 0)
            {
                return BadRequest("Invalid user id requested");
            }

            if (postId == 0)
            {
                return BadRequest("Invalid post id requested");
            }

            await _infoPostService.VoteAsync(userId, postId, false, undo);

            return Ok();
        }
    }
}
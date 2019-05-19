using Microsoft.AspNetCore.Mvc;
using Point.Services.ReturnImageService.Model;
using System.Threading.Tasks;

namespace Point.API.Controllers
{
    [Produces("application/json")]
    [Route("api/image")]
    public class ImageController : Controller
    {
        private readonly IStorageImageService _getImageService;

        public ImageController(IStorageImageService getImageService)
        {
            _getImageService = getImageService;
        }
        
        [HttpGet("get/{url}/{name}")]
        public async Task<IActionResult> GetAsync(string url, string name)
        {
            var bytes = await _getImageService.GetImageAsync(url, name);
            return File(bytes, "image/png");
        }
    }
}
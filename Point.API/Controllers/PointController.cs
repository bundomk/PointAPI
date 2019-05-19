using Microsoft.AspNetCore.Mvc;
using Point.Contracts.Models;
using Point.Services.InfoPostService.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Point.API.Controllers
{
    [Produces("application/json")]
    [Route("api/point")]
    public class PointController : Controller
    {
        private readonly string _getImageMethod = "/api/image/get";
        private readonly IInfoPostService _infoPostService;

        public PointController(IInfoPostService infoPostService)
        {
            _infoPostService = infoPostService;
        }

        [HttpGet("get/{postId}/{userId?}")]
        public async Task<PointDetail> GetAsync(long postId, long userId)
        {
            return await _infoPostService.GetAsync(GetUrl(), postId, userId);
        }

        [HttpGet("getvotes/{postId}/{userId?}")]
        public async Task<PointVotes> GetVotesAsync(long postId, long userId)
        {
            return await _infoPostService.GetVotesAsync(postId, userId);
        }

        [HttpGet("getall/{userId?}/{distance?}/{latitude?}/{longitude?}")]
        public async Task<List<PointDetail>> GetAllAsync(long userId = 0, double distance = 0, double latitude = 0, double longitude = 0)
        {
            if (distance == 0)
            {
                return await _infoPostService.GetAllAsync(GetUrl(), userId);
            }
            else
            {
                return await _infoPostService.GetAllByDistanceAsync(GetUrl(), userId, latitude, longitude, distance);
            }
        }

        [HttpGet("getalllist/{userId?}/{distance?}/{latitude?}/{longitude?}")]
        public async Task<List<PointDetail>> GetAllListAsync(long userId = 0, double distance = 0, double latitude = 0, double longitude = 0)
        {
            bool details = true;

            if (distance == 0)
            {
                return await _infoPostService.GetAllAsync(GetUrl(), userId, details);
            }
            else
            {
                return await _infoPostService.GetAllByDistanceAsync(GetUrl(), userId, latitude, longitude, distance, details);
            }
        }

        private string GetUrl()
        {
            var netsPaymentURL = new UriBuilder(Request.Scheme, Request.Host.Host, (Request.Host.Port ?? -1), _getImageMethod);

            return netsPaymentURL.Uri.AbsoluteUri;
        }
    }
}
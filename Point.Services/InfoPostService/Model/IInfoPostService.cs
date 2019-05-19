using Point.Contracts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Point.Services.InfoPostService.Model
{
    public interface IInfoPostService
    {
        Task AddAsync(InfoPost info);
        Task<PointDetail> GetAsync(string serverUrl, long postId, long userId);
        Task<PointVotes> GetVotesAsync(long postId, long userId);
        Task<List<PointDetail>> GetAllAsync(string serverUrl, long userId, bool details = false);
        Task<List<PointDetail>> GetAllByDistanceAsync(string serverUrl, long userId, double latitude, double longitude, double distance, bool details = false);
        Task<bool> IsApprovedAsync(long id, bool isApproved);
        Task VoteAsync(long userId, long postId, bool vote, bool undo);
    }
}

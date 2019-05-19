using Point.Data.Models;
using Point.Data.Repository.Model;
using System.Threading.Tasks;

namespace Point.Data.Repositories.Contracts
{
    public interface IVotePostRepository : IRepository<VotePost>
    {
        Task RemoveByPostIdUserIdAsync(long postId, long userId);
    }
}

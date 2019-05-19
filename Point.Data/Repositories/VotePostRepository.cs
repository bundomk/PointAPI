using Point.Data.Models;
using Point.Data.Repositories.Contracts;
using Point.Data.Repository;
using System.Linq;
using System.Threading.Tasks;

namespace Point.Data.Repositories
{
    public class VotePostRepository : Repository<VotePost>, IVotePostRepository
    {
        protected PointAdvisorContext Context => _context as PointAdvisorContext;

        public VotePostRepository(PointAdvisorContext context) : base(context)
        { }

        public async Task RemoveByPostIdUserIdAsync(long postId, long userId)
        {
            var old = Context.VotePost.Where(x => x.InfoPostId == postId && x.UserId == userId);
            Context.RemoveRange(old);
            await Context.SaveChangesAsync();
        }
    }
}

using System;
using System.Threading.Tasks;
using Point.Data.Models;
using Point.Data.Repositories.Contracts;
using Point.Data.Repository;

namespace Point.Data.Repositories
{
    public class InfoPostRepository : Repository<InfoPost>, IInfoPostRepository
    {
        protected PointAdvisorContext Context => _context as PointAdvisorContext;

        public InfoPostRepository(PointAdvisorContext context) : base(context)
        { }

        public async Task<bool> IsApprovedAsync(long id, bool isApproved)
        {
            bool result = false;
            try
            {
                var post = new InfoPost() { Id = id, IsApproved = isApproved, ApprovedTime = DateTime.Now };

                Context.InfoPost.Attach(post);
                Context.Entry(post).Property(x => x.IsApproved).IsModified = true;
                Context.Entry(post).Property(x => x.ApprovedTime).IsModified = true;
                await Context.SaveChangesAsync();

                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
    }
}

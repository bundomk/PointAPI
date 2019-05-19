using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Point.Data.Models;
using Point.Data.Repositories.Contracts;
using Point.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Point.Data.Repositories
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        protected PointAdvisorContext Context => _context as PointAdvisorContext;

        public ImageRepository(PointAdvisorContext context) : base(context)
        { }

        public async Task<List<Image>> GetImagesByInfoIdAsync(long id)
        {
            return await Context.Image.AsNoTracking().Where(x => x.InfoPostId.Equals(id)).ToListAsync();
        }
    }
}

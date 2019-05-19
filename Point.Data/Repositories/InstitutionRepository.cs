using Point.Data.Models;
using Point.Data.Repositories.Contracts;
using Point.Data.Repository;

namespace Point.Data.Repositories
{
    public class InstitutionRepository : Repository<Institution>, IInstitutionRepository
    {
        protected PointAdvisorContext Context => _context as PointAdvisorContext;

        public InstitutionRepository(PointAdvisorContext context) : base(context)
        { }
    }
}

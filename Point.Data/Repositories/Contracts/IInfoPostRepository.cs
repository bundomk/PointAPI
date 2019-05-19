using Point.Data.Models;
using Point.Data.Repository.Model;
using System.Threading.Tasks;

namespace Point.Data.Repositories.Contracts
{
    public interface IInfoPostRepository : IRepository<InfoPost>
    {
        Task<bool> IsApprovedAsync(long id, bool isApproved);
    }
}

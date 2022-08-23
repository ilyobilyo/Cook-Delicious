using CookDelicious.Infrastructure.Data;
using CookDelicious.Infrasturcture.Data.Common;

namespace CookDelicious.Infrasturcture.Repositories
{
    public class ApplicationDbRepository : Repository, IApplicationDbRepository
    {
        public ApplicationDbRepository(ApplicationDbContext context)
        {
            this.Context = context;
        }
    }
}

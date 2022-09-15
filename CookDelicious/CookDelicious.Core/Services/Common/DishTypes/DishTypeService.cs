using CookDelicious.Core.Contracts.Common.DishTypes;
using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CookDelicious.Core.Services.Common.DishTypes
{
    public class DishTypeService : IDishTypeService
    {
        private readonly IApplicationDbRepository repo;

        public DishTypeService(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task<DishType> GetDishTypeByName(string dishTypeName)
        {
            return await repo.All<DishType>()
                .Where(x => x.Name == dishTypeName)
                .FirstOrDefaultAsync();
        }
    }
}

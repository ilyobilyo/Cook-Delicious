using AutoMapper;
using CookDelicious.Core.Contracts.Common.DishTypes;
using CookDelicious.Core.Service.Models;
using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CookDelicious.Core.Services.Common.DishTypes
{
    public class DishTypeService : IDishTypeService
    {
        private readonly IApplicationDbRepository repo;
        private readonly IMapper mapper;

        public DishTypeService(IApplicationDbRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<DishTypeServiceModel> GetDishTypeByName(string dishTypeName)
        {
            var dishType = await repo.All<DishType>()
                .Where(x => x.Name == dishTypeName)
                .FirstOrDefaultAsync();

            return mapper.Map<DishTypeServiceModel>(dishType);
        }
    }
}

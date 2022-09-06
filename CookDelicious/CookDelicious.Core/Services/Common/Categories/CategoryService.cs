using AutoMapper;
using CookDelicious.Core.Contracts.Common.Categories;
using CookDelicious.Core.Service.Models;
using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CookDelicious.Core.Services.Common.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly IApplicationDbRepository repo;
        private readonly IMapper mapper;

        public CategoryService(IApplicationDbRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<IList<string>> GetAllCategoryNames()
        {
           return await repo.All<Category>()
                .Select(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category> GetCategoryByName(string categoryName)
        {
            return await repo.All<Category>()
                .Where(x => x.Name == categoryName)
                .FirstOrDefaultAsync();
        }
    }
}

using CookDelicious.Core.Contracts.Common.Categories;
using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CookDelicious.Core.Services.Common.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly IApplicationDbRepository repo;

        public CategoryService(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task<Category> GetCategoryByName(string categoryName)
        {
            return await repo.All<Category>()
                .Where(x => x.Name == categoryName)
                .FirstOrDefaultAsync();
        }
    }
}

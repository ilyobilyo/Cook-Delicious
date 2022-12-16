using AutoMapper;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Service.Models;
using CookDelicious.Infrasturcture.Models.Recipes;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CookDelicious.Core.Services.Admin
{
    public class RecipeServiceAdmin : IRecipeServiceAdmin
    {
        private readonly IApplicationDbRepository repo;
        private readonly IMapper mapper;

        public RecipeServiceAdmin(IApplicationDbRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<bool> DeleteRecipe(Guid id)
        {
            var recipeToDelete = await repo.All<Recipe>()
               .Where(x => x.Id == id)
               .FirstOrDefaultAsync();

            if (recipeToDelete == null)
            {
                return false;
            }

            recipeToDelete.IsDeleted = true;

            await repo.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<RecipeServiceModel>> GetUserRecipes(string id)
        {
            var userRecipes = await repo.All<Recipe>()
                .Include(x => x.Author)
                .Where(x => x.AuthorId == id && x.IsDeleted == false)
                .ToListAsync();

            return mapper.Map<IEnumerable<RecipeServiceModel>>(userRecipes);
        }
    }
}

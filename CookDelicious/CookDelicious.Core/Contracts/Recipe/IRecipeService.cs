using CookDelicious.Core.Models.Recipe;
using CookDelicious.Models;

namespace CookDelicious.Core.Contracts.Recipe
{
    public interface IRecipeService
    {
        Task<IEnumerable<AllRecipeViewModel>> GetAllRecipes(int pageNumber);
        Task<IEnumerable<ErrorViewModel>> CreateRecipe(CreateRecipeViewModel model);
    }
}

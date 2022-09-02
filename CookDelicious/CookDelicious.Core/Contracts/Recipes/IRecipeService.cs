using CookDelicious.Core.Models.Recipe;
using CookDelicious.Infrasturcture.Models.Recipes;
using CookDelicious.Models;

namespace CookDelicious.Core.Contracts.Recipes
{
    public interface IRecipeService
    {
        Task<IEnumerable<AllRecipeViewModel>> GetAllRecipes(int pageNumber);
        Task<ErrorViewModel> CreateRecipe(CreateRecipeViewModel model);
        Task<RecipePostViewModel> GetRecipeForPost(Guid id, int commentPage);
        Task<RatingViewModel> GetRecipeForSetRating(Guid id);
        Task<bool> IsRatingSet(RatingViewModel model);
        Task<Recipe> GetById(Guid id);
    }
}

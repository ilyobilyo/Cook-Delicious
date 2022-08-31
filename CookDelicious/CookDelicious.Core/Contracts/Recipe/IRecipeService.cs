using CookDelicious.Core.Models.Recipe;
using CookDelicious.Models;

namespace CookDelicious.Core.Contracts.Recipe
{
    public interface IRecipeService
    {
        Task<IEnumerable<AllRecipeViewModel>> GetAllRecipes(int pageNumber);
        Task<ErrorViewModel> CreateRecipe(CreateRecipeViewModel model);
        Task<RecipePostViewModel> GetRecipeForPost(Guid id);
        Task<RatingViewModel> GetRecipeForSetRating(Guid id);
        Task<bool> IsRatingSet(RatingViewModel model);
    }
}

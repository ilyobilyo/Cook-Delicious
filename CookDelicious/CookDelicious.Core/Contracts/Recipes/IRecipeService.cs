using CookDelicious.Core.Models.Recipe;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Infrasturcture.Models.Recipes;
using CookDelicious.Models;

namespace CookDelicious.Core.Contracts.Recipes
{
    public interface IRecipeService
    {
        Task<ErrorViewModel> CreateRecipe(CreateRecipeServiceModel model);
        Task<RecipeServiceModel> GetRecipeForPost(Guid id);
        Task<RecipeServiceModel> GetRecipeForSetRating(Guid id);
        Task<bool> IsRatingSet(RatingSetServiceModel model);
        Task<Recipe> GetById(Guid id);
        Task<(IEnumerable<RecipeServiceModel>, int)> GetAllRecipesForPageing( int pageNumber, int pageSize);
        Task<IEnumerable<RecipeCommentServiceModel>> GetRecipeCommentsPerPage(Guid Id, int commentPage, int pageSize);
    }
}

using CookDelicious.Core.Models.Recipe;
using CookDelicious.Core.Models.Sorting;
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
        Task<IEnumerable<RecipeCommentServiceModel>> GetRecipeCommentsPerPage(Guid Id, int commentPage, int pageSize);
        Task<(IEnumerable<RecipeServiceModel>, int)> GetSortRecipesForPageing(int pageNumber, int pageSize, SortServiceModel model);
        Task<(IEnumerable<RecipeServiceModel>, int)> GetSortRecipesForPageing(int pageNumber, int pageSize, string dishType, string category, bool orderByDateAsc);
    }
}

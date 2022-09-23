using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Models;

namespace CookDelicious.Core.Contracts.Recipes
{
    public interface IRecipeService
    {
        Task<ErrorViewModel> CreateRecipe(CreateRecipeServiceModel model);
        Task<RecipeServiceModel> GetRecipeForPost(Guid id);
        Task<bool> IsRatingSet(RatingSetServiceModel model);
        Task<RecipeServiceModel> GetById(Guid id);
        Task<IEnumerable<RecipeCommentServiceModel>> GetRecipeCommentsPerPage(Guid Id, int commentPage, int pageSize);
        Task<(IEnumerable<RecipeServiceModel>, int)> GetSortRecipesForPageing(int pageNumber, int pageSize, string dishType, string category, bool orderByDateAsc);
    }
}

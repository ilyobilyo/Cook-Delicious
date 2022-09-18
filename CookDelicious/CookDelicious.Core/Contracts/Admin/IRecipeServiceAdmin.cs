using CookDelicious.Core.Service.Models;

namespace CookDelicious.Core.Contracts.Admin
{
    public interface IRecipeServiceAdmin
    {
        Task DeleteRecipe(Guid id);
        Task<IEnumerable<RecipeServiceModel>> GetUserRecipes(string id);
    }
}

using CookDelicious.Core.Models.Product;
using CookDelicious.Core.Service.Models;
using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Models;

namespace CookDelicious.Core.Contracts.Product
{
    public interface IProductService
    {
        Task<IEnumerable<ProductServiceModel>> GetAllProducts(int pageNumber);
        Task<ICollection<RecipeProduct>> GetProductsForCreatingRecipe(string products, Guid recipeId);
        Task<IList<RecipeProductViewModel>> GetProductsForRecipePost(Guid id);
    }
}

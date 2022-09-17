using CookDelicious.Core.Models.Product;
using CookDelicious.Core.Service.Models;
using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Models;

namespace CookDelicious.Core.Contracts.Product
{
    public interface IProductService
    {
        Task<IEnumerable<ProductServiceModel>> GetAllProducts();
        Task<ICollection<RecipeProduct>> SetProductsForCreatingRecipe(string products, Guid recipeId);
        Task<IList<RecipeProductServiceModel>> GetProductsForRecipePost(Guid id);
        Task<(IEnumerable<ProductServiceModel>, int)> GetAllProductsForPageing(int pageNumber, int pageSize);
        Task<ProductServiceModel> GetProductById(Guid id);
    }
}

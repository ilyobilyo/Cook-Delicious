using CookDelicious.Core.Models.Product;
using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Models;

namespace CookDelicious.Core.Contracts.Product
{
    public interface IProductService
    {
        Task<IEnumerable<AllProductViewModel>> GetAllProducts(int pageNumber);
        Task<ICollection<RecipeProduct>> GetProductsForCreatingRecipe(string products, Guid recipeId);
    }
}

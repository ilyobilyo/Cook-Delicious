using CookDelicious.Core.Models.Product;
using CookDelicious.Models;

namespace CookDelicious.Core.Contracts.Product
{
    public interface IProductService
    {
        Task<IEnumerable<AllProductViewModel>> GetAllProducts(int pageNumber);
        Task<IList<ErrorViewModel>> CreateProduct(CreateProductViewModel model);
    }
}

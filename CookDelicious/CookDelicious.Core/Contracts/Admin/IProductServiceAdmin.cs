using CookDelicious.Core.Models.Admin.Product;
using CookDelicious.Core.Models.Product;
using CookDelicious.Models;

namespace CookDelicious.Core.Contracts.Admin.Product
{
    public interface IProductServiceAdmin
    {
        Task<IList<ErrorViewModel>> CreateProduct(CreateProductViewModel model);
    }
}

using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Models;

namespace CookDelicious.Core.Contracts.Admin.Product
{
    public interface IProductServiceAdmin
    {
        Task<string> CreateProduct(CreateProductInputModel model);
        Task<bool> DeleteProduct(Guid id);
    }
}

using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Models;

namespace CookDelicious.Core.Contracts.Admin.Product
{
    public interface IProductServiceAdmin
    {
        Task<IList<ErrorViewModel>> CreateProduct(CreateProductInputModel model);
        Task DeleteProduct(Guid id);
    }
}

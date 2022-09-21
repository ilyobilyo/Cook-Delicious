using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Models;

namespace CookDelicious.Core.Contracts.Admin
{
    public interface ICategoryServiceAdmin
    {
        Task<ErrorViewModel> CreateCategory(CreateCategoryInputModel model);
        Task<IEnumerable<CategoryServiceModel>> GetAllCategories();
        Task<bool> DeleteCategory(Guid id);
    }
}

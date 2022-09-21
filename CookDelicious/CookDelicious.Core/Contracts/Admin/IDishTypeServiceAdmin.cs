using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Models;

namespace CookDelicious.Core.Contracts.Admin
{
    public interface IDishTypeServiceAdmin
    {
        Task<ErrorViewModel> CreateDishType(CreateDishTypeInputModel model);
        Task<IEnumerable<DishTypeServiceModel>> GetAllDishTypes();
        Task<bool> DeleteDishType(Guid id);

    }
}

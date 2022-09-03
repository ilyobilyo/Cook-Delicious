using CookDelicious.Core.Service.Models;

namespace CookDelicious.Core.Contracts.Common.DishTypes
{
    public interface IDishTypeService
    {
        Task<DishTypeServiceModel> GetDishTypeByName(string dishTypeName);
    }
}

using CookDelicious.Infrasturcture.Models.Common;

namespace CookDelicious.Core.Contracts.Common.DishTypes
{
    public interface IDishTypeService
    {
        Task<DishType> GetDishTypeByName(string dishTypeName);
    }
}

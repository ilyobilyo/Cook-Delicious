using CookDelicious.Core.Service.Models;

namespace CookDelicious.Core.Contracts.Common.Categories
{
    public interface ICategoryService
    {
        Task<CategoryServiceModel> GetCategoryByName(string category);
        Task<IList<string>> GetAllCategoryNames();
    }
}

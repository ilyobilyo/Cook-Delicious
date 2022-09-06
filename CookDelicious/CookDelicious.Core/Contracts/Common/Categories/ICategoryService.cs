using CookDelicious.Infrasturcture.Models.Common;

namespace CookDelicious.Core.Contracts.Common.Categories
{
    public interface ICategoryService
    {
        Task<Category> GetCategoryByName(string category);
        Task<IList<string>> GetAllCategoryNames();
    }
}

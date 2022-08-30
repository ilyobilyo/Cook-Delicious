using CookDelicious.Infrasturcture.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookDelicious.Core.Contracts.Common.Categories
{
    public interface ICategoryService
    {
        Task<Category> GetCategoryByName(string category);
        Task<IList<string>> GetAllCategoryNames();
    }
}

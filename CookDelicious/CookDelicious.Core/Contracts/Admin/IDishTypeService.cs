using CookDelicious.Core.Models.Admin.DishType;
using CookDelicious.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookDelicious.Core.Contracts.Admin
{
    public interface IDishTypeService
    {
        Task<ErrorViewModel> CreateDishType(CreateDishTypeViewModel model);
    }
}

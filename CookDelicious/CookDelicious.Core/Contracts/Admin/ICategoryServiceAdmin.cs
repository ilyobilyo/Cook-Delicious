﻿using CookDelicious.Core.Models.Admin.Category;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookDelicious.Core.Contracts.Admin
{
    public interface ICategoryServiceAdmin
    {
        Task<ErrorViewModel> CreateCategory(CreateCategoryInputModel model);
    }
}

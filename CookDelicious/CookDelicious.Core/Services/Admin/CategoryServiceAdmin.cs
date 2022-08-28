using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Models.Admin.Category;
using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Infrasturcture.Repositories;
using CookDelicious.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookDelicious.Core.Services.Admin
{
    public class CategoryServiceAdmin : ICategoryServiceAdmin
    {
        private readonly IApplicationDbRepository repo;

        public CategoryServiceAdmin(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task<ErrorViewModel> CreateCategory(AddCategoryViewModel model)
        {
            ErrorViewModel error = new ErrorViewModel();

            if (model == null || model.Name == null)
            {
                error.Messages = "Name is required!";
                return error;
            }

            if (await IsCategoryExists(model))
            {
                error.Messages = $"{model.Name} is already exist.";
                return error;
            }

            var category = new Category()
            {
                Name = model.Name,
            };

            try
            {
                await repo.AddAsync(category);
                await repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                error.Messages = "Unexpected error. You cant add this product!";
            }

            return error;
        }

        private async Task<bool> IsCategoryExists(AddCategoryViewModel model)
        {
            return await repo.All<Category>()
                .AnyAsync(x => x.Name == model.Name);
        }
    }
}

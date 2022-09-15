using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Infrasturcture.Repositories;
using CookDelicious.Models;
using Microsoft.EntityFrameworkCore;

namespace CookDelicious.Core.Services.Admin
{
    public class CategoryServiceAdmin : ICategoryServiceAdmin
    {
        private readonly IApplicationDbRepository repo;

        public CategoryServiceAdmin(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task<ErrorViewModel> CreateCategory(CreateCategoryInputModel model)
        {
            ErrorViewModel error = new ErrorViewModel();

            if (model == null || model.Name == null)
            {
                error.Messages = MessageConstant.RequiredName;
                return error;
            }


            if (await IsCategoryExists(model))
            {
                error.Messages = $"{model.Name} {MessageConstant.AlreadyExist}";
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
                error.Messages = MessageConstant.UnexpectedError;
            }

            return error;
        }

        private async Task<bool> IsCategoryExists(CreateCategoryInputModel model)
        {
            return await repo.All<Category>()
                .AnyAsync(x => x.Name == model.Name);
        }
    }
}

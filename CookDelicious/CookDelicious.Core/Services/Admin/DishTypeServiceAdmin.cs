using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Infrasturcture.Repositories;
using CookDelicious.Models;
using Microsoft.EntityFrameworkCore;

namespace CookDelicious.Core.Services.Admin
{
    public class DishTypeServiceAdmin : IDishTypeServiceAdmin
    {
        private readonly IApplicationDbRepository repo;

        public DishTypeServiceAdmin(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task<ErrorViewModel> CreateDishType(CreateDishTypeInputModel model)
        {
            ErrorViewModel error = new ErrorViewModel();

            if (model == null || model.Name == null)
            {
                error.Messages = MessageConstant.RequiredName;
                return error;
            }


            if (await IsDishTypeExists(model))
            {
                error.Messages = $"{model.Name} {MessageConstant.AlreadyExist}";
                return error;
            }

            var dishType = new DishType()
            {
                Name = model.Name,
            };


            try
            {
                await repo.AddAsync(dishType);
                await repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                error.Messages = RecipeConstants.UnexpectedErrorDishtype;
            }

            return error;
        }

        private async Task<bool> IsDishTypeExists(CreateDishTypeInputModel model)
        {
            return await repo.All<DishType>()
               .AnyAsync(x => x.Name == model.Name);
        }
    }
}

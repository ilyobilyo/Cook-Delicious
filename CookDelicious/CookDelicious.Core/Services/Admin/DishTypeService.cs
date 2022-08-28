using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Models.Admin.DishType;
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
    public class DishTypeService : IDishTypeService
    {
        private readonly IApplicationDbRepository repo;

        public DishTypeService(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task<ErrorViewModel> CreateDishType(CreateDishTypeViewModel model)
        {
            ErrorViewModel error = new ErrorViewModel();

            if (model == null || model.Name == null)
            {
                error.Messages = "Name is required!";
                return error;
            }


            if (await IsDishTypeExists(model))
            {
                error.Messages = $"{model.Name} is already exist.";
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
                error.Messages = "Unexpected error. You cant add this product!";
            }

            return error;
        }

        private async Task<bool> IsDishTypeExists(CreateDishTypeViewModel model)
        {
            return await repo.All<DishType>()
               .AnyAsync(x => x.Name == model.Name);
        }
    }
}

using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin.Product;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Infrasturcture.Repositories;
using CookDelicious.Models;
using Microsoft.EntityFrameworkCore;

namespace CookDelicious.Core.Services.Products
{
    public class ProductServiceAdmin : IProductServiceAdmin
    {
        private readonly IApplicationDbRepository repo;

        public ProductServiceAdmin(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task<string> CreateProduct(CreateProductInputModel model)
        {
            ErrorViewModel error = new ErrorViewModel();

            if (model == null || model.Name == null || model.Type == null || model.Description == null)
            {
                 return  RecipeConstants.AllFieldsAreRequired ;
               
            }

            if (await IsProductExists(model))
            {
                return $"{model.Name} {MessageConstant.AlreadyExist}";
                
            }

            var product = new Product()
            {
                Name = model.Name,
                Type = model.Type,
                ImageUrl = model.ImageUrl,
                Description = model.Description
            };

            try
            {
                await repo.AddAsync(product);
                await repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                return RecipeConstants.UnexpectedErrorProduct;
            }

            return null;
        }

        public async Task<bool> DeleteProduct(Guid id)
        {
            var productToDelete = await repo.All<Product>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (productToDelete == null)
            {
                return false;
            }

            productToDelete.IsDeleted = true;

            await repo.SaveChangesAsync();

            return true;
        }

        private async Task<bool> IsProductExists(CreateProductInputModel model)
        {
            return await repo.All<Product>()
                .AnyAsync(x => x.Name == model.Name && x.Type == model.Type);
        }
    }
}

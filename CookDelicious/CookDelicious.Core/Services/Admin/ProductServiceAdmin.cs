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

        public async Task<IList<ErrorViewModel>> CreateProduct(CreateProductInputModel model)
        {
            List<ErrorViewModel> errors = new List<ErrorViewModel>();

            if (model == null || model.Name == null || model.Type == null)
            {
                errors.Add(new ErrorViewModel() { Messages = "All fields is required!" });
                return errors;
            }

            if (await IsProductExists(model))
            {
                errors.Add(new ErrorViewModel() { Messages = $"{model.Name} is already exist." });
                return errors;
            }

            var product = new Product()
            {
                Name = model.Name,
                Type = model.Type,
            };

            try
            {
                await repo.AddAsync(product);
                await repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                errors.Add(new ErrorViewModel() { Messages = "Unexpected error. You cant add this product!" });
            }

            return errors;
        }

        private async Task<bool> IsProductExists(CreateProductInputModel model)
        {
            return await repo.All<Product>()
                .AnyAsync(x => x.Name == model.Name && x.Type == model.Type);
        }
    }
}

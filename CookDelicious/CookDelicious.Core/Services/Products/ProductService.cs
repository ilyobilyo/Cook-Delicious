using CookDelicious.Core.Contracts.Product;
using CookDelicious.Core.Models.Product;
using CookDelicious.Infrasturcture.Repositories;
using CookDelicious.Infrasturcture.Models.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Models;

namespace CookDelicious.Core.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IApplicationDbRepository repo;

        public ProductService(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IList<ErrorViewModel>> CreateProduct(CreateProductViewModel model)
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

        public async Task<IEnumerable<AllProductViewModel>> GetAllProducts(int pageNumber)
        {
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            int pageSize = 9;

            return await PagingList<AllProductViewModel>.CreateAsync(repo.All<Product>()
                .Select(p => new AllProductViewModel
                {
                    Type = p.Type,
                    Name = p.Name,
                }),
                pageNumber,
                pageSize);
        }

        private async Task<bool> IsProductExists(CreateProductViewModel model)
        {
            return await repo.All<Product>()
                .AnyAsync(x => x.Name == model.Name && x.Type == model.Type);
        }
    }
}

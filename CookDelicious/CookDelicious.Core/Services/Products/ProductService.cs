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

        public async Task<ICollection<RecipeProduct>> GetProductsForCreatingRecipe(string products, Guid recipeId)
        {
            var splitedProductsNames = products.Split(", ", StringSplitOptions.RemoveEmptyEntries);

            List<RecipeProduct> recipeProducts = new List<RecipeProduct>();

            foreach (var recipeProduct in splitedProductsNames)
            {
                var isExist = await IsProductExists(recipeProduct);

                if (isExist)
                {
                    var product = await repo.All<Product>()
                        .Where(x => x.Name == recipeProduct)
                        .FirstOrDefaultAsync();

                    recipeProducts.Add(new RecipeProduct()
                    {
                        Product = product,
                        ProductId = product.Id,
                        RecipeId = recipeId
                    });
                }
                else
                {
                    var product = await CreateProduct(recipeProduct);

                    recipeProducts.Add(new RecipeProduct()
                    {
                        Product = product,
                        ProductId = product.Id,
                        RecipeId = recipeId
                    });
                }
            }
            

            return recipeProducts;
        }

        private async Task<Product> CreateProduct(string name)
        {
            var product = new Product()
            {
                Name = name,
                Type = "Незададен",
            };

            try
            {
                await repo.AddAsync(product);
                await repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                return null;
            }

            return product;
        }

        private async Task<bool> IsProductExists(string name)
        {
            return await repo.All<Product>()
                .AnyAsync(x => x.Name == name);
        }
    }
}

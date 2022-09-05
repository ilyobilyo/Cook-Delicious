using CookDelicious.Core.Contracts.Product;
using CookDelicious.Core.Models.Product;
using CookDelicious.Infrasturcture.Repositories;
using CookDelicious.Infrasturcture.Models.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Models;
using CookDelicious.Core.Service.Models;
using AutoMapper;

namespace CookDelicious.Core.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IApplicationDbRepository repo;
        private readonly IMapper mapper;

        public ProductService(IApplicationDbRepository repo,
            IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }


        public async Task<IEnumerable<ProductServiceModel>> GetAllProducts()
        {
            var allProducts = await repo.All<Product>()
                .ToListAsync();

            return mapper.Map<IEnumerable<ProductServiceModel>>(allProducts);
        }

        public async Task<(IEnumerable<ProductServiceModel>, int)> GetAllProductsForPageing(int pageNumber, int pageSize)
        {
            var totalCount = await repo.All<Product>().CountAsync();

            var items = await repo.All<Product>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var itemsAsServiceModel = mapper.Map<IEnumerable<ProductServiceModel>>(items);

            return (itemsAsServiceModel, totalCount);
        }

        public async Task<ICollection<RecipeProduct>> GetProductsForCreatingRecipe(string products, Guid recipeId)
        {
            var splitedProductsWithQuantity = products.Split(", ", StringSplitOptions.RemoveEmptyEntries);
            var splitedProductsNames = products.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var productDict = Enumerable.Range(0, splitedProductsNames.Length / 2).ToDictionary(i => splitedProductsNames[2 * i] , i => splitedProductsNames[2 * i + 1]);

            List<RecipeProduct> recipeProducts = new List<RecipeProduct>();

            foreach (var (quantity, recipeProduct) in productDict)
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
                        RecipeId = recipeId,
                        Quantity = quantity,
                    });
                }
                else
                {
                    var product = await CreateProduct(recipeProduct);

                    recipeProducts.Add(new RecipeProduct()
                    {
                        Product = product,
                        ProductId = product.Id,
                        RecipeId = recipeId,
                        Quantity = quantity,
                    });
                }
            }


            return recipeProducts;
        }

        public async Task<IList<RecipeProductViewModel>> GetProductsForRecipePost(Guid id)
        {
            return await repo.All<RecipeProduct>()
                .Where(x => x.RecipeId == id)
                .Select(x => new RecipeProductViewModel()
                {
                    Id = x.ProductId,
                    Name = x.Product.Name,
                    Quantity = x.Quantity
                })
                .ToListAsync();
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

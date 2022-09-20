using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Product;
using CookDelicious.Core.Service.Models;
using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.EntityFrameworkCore;

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
                .Where(x => x.IsDeleted == false && x.Type != RecipeConstants.UnsetProductType)
                .ToListAsync();

            return mapper.Map<IEnumerable<ProductServiceModel>>(allProducts);
        }

        public async Task<(IEnumerable<ProductServiceModel>, int)> GetAllProductsForPageing(int pageNumber, int pageSize)
        {
            var totalCount = await repo.All<Product>().CountAsync();

            var items = await repo.All<Product>()
                .Where(x => x.IsDeleted == false && x.Type != RecipeConstants.UnsetProductType)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var itemsAsServiceModel = mapper.Map<IEnumerable<ProductServiceModel>>(items);

            return (itemsAsServiceModel, totalCount);
        }

        public async Task<ICollection<RecipeProduct>> SetProductsForCreatingRecipe(string products, Guid recipeId)
        {
            var validProducts = products.Replace(",", ", ");

            var splitedProductsWithQuantity = validProducts.Split(", ", StringSplitOptions.RemoveEmptyEntries);

            if (splitedProductsWithQuantity.Count() <= 1)
            {
                throw new ArgumentException(MessageConstant.InvalidSetRecipeProductErrorMessage);
            }

            var splitedProductsNames = products.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var productDict = Enumerable.Range(0, splitedProductsNames.Length / 2).ToDictionary(i => splitedProductsNames[2 * i + 1] , i => splitedProductsNames[2 * i]);

            List<RecipeProduct> recipeProducts = new List<RecipeProduct>();

            foreach (var (recipeProduct, quantity) in productDict)
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

        public async Task<ProductServiceModel> GetProductById(Guid id)
        {
            var product = await repo.All<Product>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return mapper.Map<ProductServiceModel>(product);
        }

        public async Task<IList<RecipeProductServiceModel>> GetProductsForRecipePost(Guid id)
        {
            var recipeProducts = await repo.All<RecipeProduct>()
                .Where(x => x.RecipeId == id)
                .ToListAsync();

            return mapper.Map<List<RecipeProductServiceModel>>(recipeProducts);
        }

        private async Task<Product> CreateProduct(string name)
        {
            var product = new Product()
            {
                Name = name,
                Type = RecipeConstants.UnsetProductType,
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

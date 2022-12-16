using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Product;
using CookDelicious.Core.MapProfiles;
using CookDelicious.Core.Services.Products;
using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Infrasturcture.Models.Identity;
using CookDelicious.Infrasturcture.Models.Recipes;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookDelicious.Tests
{
    public class ProductServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IApplicationDbRepository, ApplicationDbRepository>()
                .AddSingleton<IProductService, ProductService>()
                .AddAutoMapper(typeof(UserMapping),
                                typeof(ProductMapping))
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();

            await SeedAsync(repo);
        }

        [Test]
        public async Task SucceedGetAllProducts()
        {
            var service = serviceProvider.GetService<IProductService>();

            var products = await service.GetAllProducts();

            var productsList = products.ToList();

            Assert.That(productsList.Count == 5);
        }

        [Test]
        public async Task SucceedGetAllProductsPerPage()
        {
            var service = serviceProvider.GetService<IProductService>();

            var products = await service.GetAllProductsForPageing(1, 3);

            Assert.That(products.Items.Count() == 3 && products.TotalCount == 5);
        }

        [Test]
        public async Task SucceedSetProductsForCreatingRecipe()
        {
            var service = serviceProvider.GetService<IProductService>();

            var recipeid = Guid.Parse("d86dac3a-e8ca-4205-b99d-fa0d44cfbd74");

            var products = "20gr luk, 20gr chesun";

            var recipeProducts = await service.SetProductsForCreatingRecipe(products, recipeid);

            Assert.That(recipeProducts.Count == 2);
        }

        [Test]
        public async Task SetProductsForCreatingRecipeThrowsArgumentExeption()
        {
            var service = serviceProvider.GetService<IProductService>();

            var recipeid = Guid.Parse("d86dac3a-e8ca-4205-b99d-fa0d44cfbd74");

            var products = "20gr luk";

            Assert.CatchAsync<ArgumentException>(async () => await service.SetProductsForCreatingRecipe(products, recipeid), MessageConstant.InvalidSetRecipeProductErrorMessage);
        }

        [Test]
        public async Task SucceedGetProductById()
        {
            var service = serviceProvider.GetService<IProductService>();

            var id = Guid.Parse("caccc889-f2ec-4538-9c3b-90540dee23f1");

            var product = await service.GetProductById(id);

            Assert.That(product != null);
        }

        [Test]
        public async Task GetProductByIdWithInvalidId()
        {
            var service = serviceProvider.GetService<IProductService>();

            var id = Guid.Parse("caccc889-f2ec-4538-9c3b-90540dee2341");

            var product = await service.GetProductById(id);

            Assert.That(product == null);
        }

        [Test]
        public async Task SucceedGetProductsForRecipe()
        {
            var service = serviceProvider.GetService<IProductService>();

            var recipeId = Guid.Parse("d86dac3a-e8ca-4205-b99d-fa0d44cfbd74");

            var products = await service.GetProductsForRecipePost(recipeId);

            Assert.That(products.Count == 2);
        }

        [Test]
        public async Task GetProductsForRecipeWithInvalidRecipeId()
        {
            var service = serviceProvider.GetService<IProductService>();

            var recipeId = Guid.Parse("d861ac3a-e8ca-4205-b99d-fa0d44cfbd74");

            var products = await service.GetProductsForRecipePost(recipeId);

            Assert.That(products.Count == 0);
        }



        private async Task SeedAsync(IApplicationDbRepository repo)
        {
            var product = new Product()
            {
                Id = new Guid("caccc889-f2ec-4538-9c3b-90540dee23f1"),
                Description = "TestDescription",
                ImageUrl = "TestUrl",
                Name = "TestProduct1",
                Type = "TestType"
            };

            var product2 = new Product()
            {
                Id = new Guid("caccc889-f2ec-4538-9c3b-90540dee23f2"),
                Description = "TestDescription",
                ImageUrl = "TestUrl",
                Name = "TestProduct2",
                Type = "TestType"
            };

            var product3 = new Product()
            {
                Id = new Guid("caccc889-f2ec-4538-9c3b-90540dee23f3"),
                Description = "TestDescription",
                ImageUrl = "TestUrl",
                Name = "TestProduct3",
                Type = "TestType"
            };

            var product4 = new Product()
            {
                Id = new Guid("caccc889-f2ec-4538-9c3b-90540dee23f4"),
                Description = "TestDescription",
                ImageUrl = "TestUrl",
                Name = "TestProduct4",
                Type = "TestType"
            };

            var product5 = new Product()
            {
                Id = new Guid("caccc889-f2ec-4538-9c3b-90540dee23f5"),
                Description = "TestDescription",
                ImageUrl = "TestUrl",
                Name = "TestProduct5",
                Type = "TestType"
            };


            var user = new ApplicationUser()
            {
                Id = "TestId",
                Address = "TestAddress",
                Age = 20,
                Email = "test@gmail.com",
                EmailConfirmed = true,
                FirstName = "Dave",
                ImageUrl = "testUrl",
                Job = "TestJob",
                LastName = "Daveson",
                Town = "Varna",
                UserName = "TestUsername",
            };

            var dishType = new DishType()
            {
                Name = "TestDishType",
            };

            var category = new Category()
            {
                Name = "TestCategory"
            };

            var recipe = new Recipe()
            {
                Id = new Guid("d86dac3a-e8ca-4205-b99d-fa0d44cfbd74"),
                Author = user,
                AuthorId = user.Id,
                CategoryId = category.Id,
                Catrgory = category,
                CookingTime = "10 min",
                Description = "Test Description",
                DishType = dishType,
                DishTypeId = dishType.Id,
                ImageUrl = "TestUrl",
                PublishedOn = System.DateTime.UtcNow,
                Title = "TestRecipe",
            };

            var recipeProducts = new List<RecipeProduct>()
            {
                new RecipeProduct
                {
                    Recipe = recipe,
                    Quantity = "20",
                    Product = product
                },

                new RecipeProduct
                {
                    Recipe = recipe,
                    Product = product2,
                    Quantity = "30"
                }
            };

            recipe.RecipeProducts = recipeProducts;

            await repo.AddAsync(product);
            await repo.AddAsync(product2);
            await repo.AddAsync(product3);
            await repo.AddAsync(product4);
            await repo.AddAsync(product5);
            await repo.AddAsync(user);
            await repo.AddAsync(category);
            await repo.AddAsync(dishType);
            await repo.AddAsync(recipe);
            await repo.SaveChangesAsync();
        }
    }
}

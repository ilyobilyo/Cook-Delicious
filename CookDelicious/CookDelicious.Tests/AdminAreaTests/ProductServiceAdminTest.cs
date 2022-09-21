using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin.Product;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Core.Services.Products;
using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookDelicious.Tests.AdminAreaTests
{
    public class ProductServiceAdminTest
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
                .AddSingleton<IProductServiceAdmin, ProductServiceAdmin>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();

            await SeedAsync(repo);
        }

        [Test]
        public async Task SucceedCreateProduct()
        {
            var inputModel = new CreateProductInputModel()
            {
               Type ="testType",
               Description = "desc",
               ImageUrl = "Url",
               Name = "testProduct"
            };

            var service = serviceProvider.GetService<IProductServiceAdmin>();

            var error = await service.CreateProduct(inputModel);

            Assert.That(error.Messages == null);
        }

        [Test]
        public async Task CreateProductInvalidName()
        {
            var inputModel = new CreateProductInputModel()
            {
                Type = "testType",
                Description = "desc",
                ImageUrl = "Url",
                Name = null
            };

            var service = serviceProvider.GetService<IProductServiceAdmin>();

            var error = await service.CreateProduct(inputModel);

            Assert.That(error.Messages == RecipeConstants.AllFieldsAreRequired);
        }

        [Test]
        public async Task CreateProductInvalidType()
        {
            var inputModel = new CreateProductInputModel()
            {
                Type = null,
                Description = "desc",
                ImageUrl = "Url",
                Name = "testProduct"
            };

            var service = serviceProvider.GetService<IProductServiceAdmin>();

            var error = await service.CreateProduct(inputModel);

            Assert.That(error.Messages == RecipeConstants.AllFieldsAreRequired);
        }

        [Test]
        public async Task CreateProductInvalidDescription()
        {
            var inputModel = new CreateProductInputModel()
            {
                Type = "testType",
                Description = null,
                ImageUrl = "Url",
                Name = "testProduct"
            };

            var service = serviceProvider.GetService<IProductServiceAdmin>();

            var error = await service.CreateProduct(inputModel);

            Assert.That(error.Messages == RecipeConstants.AllFieldsAreRequired);
        }

        [Test]
        public async Task CreateProductThatAlreadyExists()
        {
            var inputModel = new CreateProductInputModel()
            {
                Type = "TestType",
                Description = "asdasd",
                ImageUrl = "Url",
                Name = "TestProduct"
            };

            var service = serviceProvider.GetService<IProductServiceAdmin>();

            var error = await service.CreateProduct(inputModel);

            Assert.That(error.Messages == $"{inputModel.Name} {MessageConstant.AlreadyExist}");
        }

        [Test]
        public async Task SucceedDeleteProduct()
        {
            var id = Guid.Parse("06e3cdd6-a590-44cc-8861-816cf139d7db");

            var service = serviceProvider.GetService<IProductServiceAdmin>();

            var isDeleted = await service.DeleteProduct(id);

            Assert.That(isDeleted == true);
        }

        [Test]
        public async Task DeleteProductWithInvalidId()
        {
            var id = Guid.Parse("06e32dd6-a590-44cc-8861-816cf139d7db");

            var service = serviceProvider.GetService<IProductServiceAdmin>();

            var isDeleted = await service.DeleteProduct(id);

            Assert.That(isDeleted == false);
        }


        private async Task SeedAsync(IApplicationDbRepository repo)
        {

            var product = new Product()
            {
                Id = new Guid("06e3cdd6-a590-44cc-8861-816cf139d7db"),
                Name = "TestProduct",
                Description = "TestDesc",
                ImageUrl = "TestUrl",
                Type = "TestType"
            };

            var product2 = new Product()
            {
                Id = new Guid("16e3cdd6-a590-44cc-8861-816cf139d7db"),
                Name = "TestProduct2",
                Description = "TestDesc",
                ImageUrl = "TestUrl",
                Type = "TestType"
            };

            await repo.AddAsync(product);
            await repo.AddAsync(product2);
            await repo.SaveChangesAsync();
        }
    }
}

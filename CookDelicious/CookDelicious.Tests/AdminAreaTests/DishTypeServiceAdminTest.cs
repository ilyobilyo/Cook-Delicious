using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.MapProfiles;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Core.Services.Admin;
using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CookDelicious.Tests.AdminAreaTests
{
    public class DishTypeServiceAdminTest
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
                .AddSingleton<IDishTypeServiceAdmin, DishTypeServiceAdmin>()
                .AddAutoMapper(typeof(DishTypeMapping))
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();

            await SeedAsync(repo);
        }

        [Test]
        public async Task SucceedCreateDishType()
        {
            var inputModel = new CreateDishTypeInputModel()
            {
                Name = "TestInputDishType"
            };

            var service = serviceProvider.GetService<IDishTypeServiceAdmin>();

            var error = await service.CreateDishType(inputModel);

            Assert.That(error.Messages == null);
        }

        [Test]
        public async Task CreateDishTypeWithInvalidName()
        {
            var inputModel = new CreateDishTypeInputModel()
            {
                Name = null
            };

            var service = serviceProvider.GetService<IDishTypeServiceAdmin>();

            var error = await service.CreateDishType(inputModel);

            Assert.That(error.Messages == MessageConstant.RequiredName);
        }

        [Test]
        public async Task CreateCategoryThatAlreadyExist()
        {
            var inputModel = new CreateDishTypeInputModel()
            {
                Name = "TestDishType"
            };

            var service = serviceProvider.GetService<IDishTypeServiceAdmin>();

            var error = await service.CreateDishType(inputModel);

            Assert.That(error.Messages == $"{inputModel.Name} {MessageConstant.AlreadyExist}");
        }

        [Test]
        public async Task SucceedDeleteDishType()
        {
            var id = Guid.Parse("06e3cdd6-a590-44cc-8861-816cf139d7db");

            var service = serviceProvider.GetService<IDishTypeServiceAdmin>();

            var isDeleted = await service.DeleteDishType(id);

            Assert.That(isDeleted == true);
        }

        [Test]
        public async Task DeleteDishTypeWithInvalidId()
        {
            var id = Guid.Parse("06e3cdd6-a590-44cc-8861-816cf13927db");

            var service = serviceProvider.GetService<IDishTypeServiceAdmin>();

            var isDeleted = await service.DeleteDishType(id);

            Assert.That(isDeleted == false);
        }

        [Test]
        public async Task SucceedGetAllDishTypes()
        {

            var service = serviceProvider.GetService<IDishTypeServiceAdmin>();

            var allCategories = await service.GetAllDishTypes();

            var allCategoriesList = allCategories.ToList();

            Assert.That(allCategoriesList.Count == 2);
        }



        private async Task SeedAsync(IApplicationDbRepository repo)
        {

            var dishType = new DishType()
            {
                Id = new Guid("06e3cdd6-a590-44cc-8861-816cf139d7db"),
                Name = "TestDishType"
            };

            var dishType2 = new DishType()
            {
                Name = "TestDishType2"
            };

            await repo.AddAsync(dishType);
            await repo.AddAsync(dishType2);
            await repo.SaveChangesAsync();
        }
    }
}

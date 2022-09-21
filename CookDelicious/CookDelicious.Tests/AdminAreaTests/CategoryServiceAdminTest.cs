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
    public class CategoryServiceAdminTest
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
                .AddSingleton<ICategoryServiceAdmin, CategoryServiceAdmin>()
                .AddAutoMapper(typeof(CategoryMapping))
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();

            await SeedAsync(repo);
        }

        [Test]
        public async Task SucceedCreateCategory()
        {
            var inputModel = new CreateCategoryInputModel()
            {
                Name = "TestInputCategory"
            };

            var service = serviceProvider.GetService<ICategoryServiceAdmin>();

            var error = await service.CreateCategory(inputModel);

            Assert.That(error.Messages == null);
        }

        [Test]
        public async Task CreateCategoryWithInvalidName()
        {
            var inputModel = new CreateCategoryInputModel()
            {
                Name = null
            };

            var service = serviceProvider.GetService<ICategoryServiceAdmin>();

            var error = await service.CreateCategory(inputModel);

            Assert.That(error.Messages == MessageConstant.RequiredName);
        }

        [Test]
        public async Task CreateCategoryThatAlreadyExist()
        {
            var inputModel = new CreateCategoryInputModel()
            {
                Name = "TestCategory"
            };

            var service = serviceProvider.GetService<ICategoryServiceAdmin>();

            var error = await service.CreateCategory(inputModel);

            Assert.That(error.Messages == $"{inputModel.Name} {MessageConstant.AlreadyExist}");
        }

        [Test]
        public async Task SucceedDeleteCategory()
        {
            var id = Guid.Parse("06e3cdd6-a590-44cc-8861-816cf139d7db");

            var service = serviceProvider.GetService<ICategoryServiceAdmin>();

            var isDeleted = await service.DeleteCategory(id);

            Assert.That(isDeleted == true);
        }

        [Test]
        public async Task DeleteCategoryWithInvalidId()
        {
            var id = Guid.Parse("06e3cdd6-a190-44cc-8861-816cf139d7db");

            var service = serviceProvider.GetService<ICategoryServiceAdmin>();

            var isDeleted = await service.DeleteCategory(id);

            Assert.That(isDeleted == false);
        }

        [Test]
        public async Task SucceedGetAllCategories()
        {

            var service = serviceProvider.GetService<ICategoryServiceAdmin>();

            var allCategories = await service.GetAllCategories();

            var allCategoriesList = allCategories.ToList();

            Assert.That(allCategoriesList.Count == 2);
        }





        private async Task SeedAsync(IApplicationDbRepository repo)
        {

            var category = new Category()
            {
                Id = new Guid("06e3cdd6-a590-44cc-8861-816cf139d7db"),
                Name = "TestCategory"
            };

            var category2 = new Category()
            {
                Name = "TestCategory2"
            };

            await repo.AddAsync(category);
            await repo.AddAsync(category2);
            await repo.SaveChangesAsync();
        }
    }
}

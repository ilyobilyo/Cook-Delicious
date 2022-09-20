using CookDelicious.Core.Contracts.Common.Categories;
using CookDelicious.Core.Services.Common.Categories;
using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CookDelicious.Tests
{
    public class CategoryServiceTest
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
                .AddSingleton<ICategoryService, CategoryService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();

            await SeedAsync(repo);
        }

        [Test]
        public async Task SucceedGetAllCategoryNames()
        {
            var service = serviceProvider.GetService<ICategoryService>();

            var categories = await service.GetAllCategoryNames();

            Assert.That(categories.Count == 3);
        }

        [Test]
        public async Task SucceedGetCategoryByName()
        {
            var service = serviceProvider.GetService<ICategoryService>();

            var categoryName = "TestCategory2";

            var category = await service.GetCategoryByName(categoryName);

            Assert.That(category != null);
        }

        [Test]
        public async Task GetCategoryByNameWithInvalidName()
        {
            var service = serviceProvider.GetService<ICategoryService>();

            var categoryName = "asdasdasdasd";

            var category = await service.GetCategoryByName(categoryName);

            Assert.That(category == null);
        }


        private async Task SeedAsync(IApplicationDbRepository repo)
        {
            var category = new Category()
            {
                Name = "TestCategory"
            };

            var category2 = new Category()
            {
                Name = "TestCategory2"
            };

            var category3 = new Category()
            {
                Name = "TestCategory3"
            };

            await repo.AddAsync(category);
            await repo.AddAsync(category2);
            await repo.AddAsync(category3);
            await repo.SaveChangesAsync();
        }
    }
}

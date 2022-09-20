using CookDelicious.Core.Contracts.Common.DishTypes;
using CookDelicious.Core.Services.Common.DishTypes;
using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CookDelicious.Tests
{
    public class DishTypeServiceTest
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
                .AddSingleton<IDishTypeService, DishTypeService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();

            await SeedAsync(repo);
        }

        [Test]
        public async Task SucceedGetDishTypeByName()
        {
            var service = serviceProvider.GetService<IDishTypeService>();

            var dishTypeName = "TestDishType3";

            var dishType = await service.GetDishTypeByName(dishTypeName);

            Assert.That(dishType != null);
        }

        [Test]
        public async Task GetDishTypeWithInvalidName()
        {
            var service = serviceProvider.GetService<IDishTypeService>();

            var dishTypeName = "asdasdasdasd";

            var dishType = await service.GetDishTypeByName(dishTypeName);

            Assert.That(dishType == null);
        }

        private async Task SeedAsync(IApplicationDbRepository repo)
        {
            var dishType = new DishType()
            {
                Name = "TestDishType"
            };

            var dishType2 = new DishType()
            {
                Name = "TestDishType2"
            };

            var dishType3 = new DishType()
            {
                Name = "TestDishType3"
            };

            await repo.AddAsync(dishType);
            await repo.AddAsync(dishType2);
            await repo.AddAsync(dishType3);
            await repo.SaveChangesAsync();
        }
    }
}

using CookDelicious.Core.Contracts.User;
using CookDelicious.Core.MapProfiles;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Core.Services.User;
using CookDelicious.Infrasturcture.Models.Identity;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CookDelicious.Tests
{
    public class UserServiceTest
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
                .AddSingleton<IUserService, UserService>()
                .AddAutoMapper(typeof(UserMapping))
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();

            await SeedAsync(repo);
        }
        
        [Test]
        public async Task SucceedGetApplicationUserByUsername()
        {
            var username = "TestUsername";

            var service = serviceProvider.GetService<IUserService>();

            var user = await service.GetApplicationUserByUsername(username);

            Assert.That(user != null);
        }

        [Test]
        public async Task GetApplicationUserByUsernameWithInvalidUsername()
        {
            var username = "asdasadasdasdasdasdasdasdsd";

            var service = serviceProvider.GetService<IUserService>();

            var user = await service.GetApplicationUserByUsername(username);

            Assert.That(user == null);
        }

        [Test]
        public async Task SucceedGetUserServiceModelUserByUsername()
        {
            var username = "TestUsername";

            var service = serviceProvider.GetService<IUserService>();

            var user = await service.GetUserByUsername(username);

            Assert.That(user != null && user.Recipes != null);
        }

        [Test]
        public async Task GetUserServiceModelByUsernameWithInvalidUsername()
        {
            var username = "asdasadasdasdasdasdasdasdsd";

            var service = serviceProvider.GetService<IUserService>();

            var user = await service.GetUserByUsername(username);

            Assert.That(user == null);
        }

        [Test]
        public async Task SucceedUpdateUser()
        {
            var editUserData = new UserEditProfileInputModel()
            {
                Address = "testaddres",
                Age = 12,
                Email = "testEmail",
                FirstName = "dfoe",
                Id = "TestId",
                ImageUrl = "TestUrl",
                Job = "TestJob",
                LastName = "LastTestName",
                Town = "spfia",
                Username = "rick"
            };

            var service = serviceProvider.GetService<IUserService>();

            var isUpdated = await service.UpdateUser(editUserData);

            Assert.That(isUpdated == true);
        }

        [Test]
        public async Task UpdateUserWuthInvalidUserId()
        {
            var editUserData = new UserEditProfileInputModel()
            {
                Address = "testaddres",
                Age = 12,
                Email = "testEmail",
                FirstName = "dfoe",
                Id = "asdasdasdasdasd",
                ImageUrl = "TestUrl",
                Job = "TestJob",
                LastName = "LastTestName",
                Town = "spfia",
                Username = "rick"
            };

            var service = serviceProvider.GetService<IUserService>();

            var isUpdated = await service.UpdateUser(editUserData);

            Assert.That(isUpdated == false);
        }




        private async Task SeedAsync(IApplicationDbRepository repo)
        {
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

            await repo.AddAsync(user);
            await repo.SaveChangesAsync();
        }
    }
}

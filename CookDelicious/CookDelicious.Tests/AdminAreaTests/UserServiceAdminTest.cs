using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.MapProfiles;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Core.Services.Admin;
using CookDelicious.Infrasturcture.Models.Identity;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace CookDelicious.Tests.AdminAreaTests
{
    public class UserServiceAdminTest
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
                .AddSingleton<IUserServiceAdmin, UserServiceAdmin>()
                .AddAutoMapper(typeof(UserMapping))
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();

            await SeedAsync(repo);
        }

        [Test]
        public async Task SucceedGetUserByIdForEditIsUserServiceModel()
        {
            var service = serviceProvider.GetService<IUserServiceAdmin>();

            var userId = "TestId";

            var user = await service.GetUserByIdEdit(userId);

            Assert.IsInstanceOf(typeof(UserServiceModel), user);
        }

        [Test]
        public async Task SucceedGetUserByIdForEdit()
        {
            var service = serviceProvider.GetService<IUserServiceAdmin>();

            var userId = "TestId";

            var user = await service.GetUserByIdEdit(userId);

            Assert.That(user != null);
        }

        [Test]
        public async Task GetUserByIdForEditWithInvalidId()
        {
            var service = serviceProvider.GetService<IUserServiceAdmin>();

            var userId = "Tasdasdasd";

            var user = await service.GetUserByIdEdit(userId);

            Assert.That(user == null);
        }

        [Test]
        public async Task SucceedGetUserByIdForRolesIsApplicationUser()
        {
            var service = serviceProvider.GetService<IUserServiceAdmin>();

            var userId = "TestId";

            var user = await service.GetUserByIdRoles(userId);

            Assert.IsInstanceOf(typeof(ApplicationUser), user);
        }

        [Test]
        public async Task SucceedGetUserByIdForRoles()
        {
            var service = serviceProvider.GetService<IUserServiceAdmin>();

            var userId = "TestId";

            var user = await service.GetUserByIdRoles(userId);

            Assert.That(user != null);
        }

        [Test]
        public async Task GetUserByIdForRolesWithInvalidId()
        {
            var service = serviceProvider.GetService<IUserServiceAdmin>();

            var userId = "Tasdasdasd";

            var user = await service.GetUserByIdRoles(userId);

            Assert.That(user == null);
        }

        [Test]
        public async Task SucceedGetUsersPageingInManageUsers()
        {
            int pageNumber = 1;

            int pageSize = 2;

            var service = serviceProvider.GetService<IUserServiceAdmin>();

            var users = await service.GetUsersPageingInManageUsers(pageNumber, pageSize);

            Assert.That(users.Items.Count() == 2 && users.TotalCount == 3);
        }

        [Test]
        public async Task SucceedUpdateUser()
        {
            var model = new UpdateUserInputModel()
            {
                Id = "TestId",
                Username = "new username"
            };

            var service = serviceProvider.GetService<IUserServiceAdmin>();

            var isUpdated = await service.UpdateUser(model);

            Assert.That(isUpdated == true);
        }

        [Test]
        public async Task UpdateUserWithInvalidId()
        {
            var model = new UpdateUserInputModel()
            {
                Id = "asdasd",
                Username = "new username"
            };

            var service = serviceProvider.GetService<IUserServiceAdmin>();

            var isUpdated = await service.UpdateUser(model);

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

            var user2 = new ApplicationUser()
            {
                Id = "TestId2",
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

            var user3 = new ApplicationUser()
            {
                Id = "TestId3",
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
            await repo.AddAsync(user2);
            await repo.AddAsync(user3);

            await repo.SaveChangesAsync();
        }
    }
}

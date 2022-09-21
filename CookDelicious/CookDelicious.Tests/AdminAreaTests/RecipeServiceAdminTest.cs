using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.MapProfiles;
using CookDelicious.Core.Services.Admin;
using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Infrasturcture.Models.Identity;
using CookDelicious.Infrasturcture.Models.Recipes;
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
    public class RecipeServiceAdminTest
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
                .AddSingleton<IRecipeServiceAdmin, RecipeServiceAdmin>()
                .AddAutoMapper(typeof(RecipeMapping))
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();

            await SeedAsync(repo);
        }

        [Test]
        public async Task SucceedDeleteRecipe()
        {
            var id = Guid.Parse("4aa617e5-c701-4854-936c-26de9cb49ce8");

            var service = serviceProvider.GetService<IRecipeServiceAdmin>();

            var isDeleted = await service.DeleteRecipe(id);

            Assert.That(isDeleted == true);
        }

        [Test]
        public async Task DeleteRecipeWithInvalidId()
        {
            var id = Guid.Parse("42a617e5-c701-4854-936c-26de9cb49ce8");

            var service = serviceProvider.GetService<IRecipeServiceAdmin>();

            var isDeleted = await service.DeleteRecipe(id);

            Assert.That(isDeleted == false);
        }

        [Test]
        public async Task SucceedGetUserRecipes()
        {
            var id = "TestId";

            var service = serviceProvider.GetService<IRecipeServiceAdmin>();

            var recipes = await service.GetUserRecipes(id);

            var userRecipesList = recipes.ToList();

            Assert.That(userRecipesList.Count == 1);
        }

        [Test]
        public async Task GetUserRecipesWithInvalidId()
        {
            var id = "asdasdasd";

            var service = serviceProvider.GetService<IRecipeServiceAdmin>();

            var recipes = await service.GetUserRecipes(id);

            var userRecipesList = recipes.ToList();

            Assert.That(userRecipesList.Count == 0);
        }







        private async Task SeedAsync(IApplicationDbRepository repo)
        {

            var category = new Category()
            {
                Id = new Guid("9bb20e3d-78f1-4def-919f-67fb0f1cea14"),
                Name = "TestCategory"
            };

            var dishType = new DishType()
            {
                Name = "TestDishType",
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

            var recipe = new Recipe()
            {
                Id = new Guid("4aa617e5-c701-4854-936c-26de9cb49ce8"),
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

            await repo.AddAsync(category);
            await repo.AddAsync(dishType);
            await repo.AddAsync(user);
            await repo.AddAsync(recipe);

            await repo.SaveChangesAsync();
        }
    }
}

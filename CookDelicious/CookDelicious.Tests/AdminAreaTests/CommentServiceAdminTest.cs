using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.MapProfiles;
using CookDelicious.Core.Services.Admin;
using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Infrasturcture.Models.Forum;
using CookDelicious.Infrasturcture.Models.Identity;
using CookDelicious.Infrasturcture.Models.Recipes;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CookDelicious.Tests.AdminAreaTests
{
    public class CommentServiceAdminTest
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
                .AddSingleton<ICommentServiceAdmin, CommentServiceAdmin>()
                .AddAutoMapper(typeof(ForumMapping),
                typeof(RecipeMapping),
                typeof(UserMapping))
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();

            await SeedAsync(repo);
        }

        [Test]
        public async Task SucceedDeleteRecipeComment()
        {
            var id = Guid.Parse("33dc9d58-da70-4a37-9978-a9c2ec1abf1d");

            var service = serviceProvider.GetService<ICommentServiceAdmin>();

            var isDeleted = await service.DeleteRecipeComment(id);

            Assert.That(isDeleted == true);
        }

        [Test]
        public async Task DeleteRecipeCommentWithInvalidId()
        {
            var id = Guid.Parse("33dc9d58-da70-4a37-9978-a9c2ec1ab21d");

            var service = serviceProvider.GetService<ICommentServiceAdmin>();

            var isDeleted = await service.DeleteRecipeComment(id);

            Assert.That(isDeleted == false);
        }

        [Test]
        public async Task SucceedDeleteForumComment()
        {
            var id = Guid.Parse("5ac6e020-e48e-468a-a94b-a5cccd559cbb");

            var service = serviceProvider.GetService<ICommentServiceAdmin>();

            var isDeleted = await service.DeleteForumComment(id);

            Assert.That(isDeleted == true);
        }

        [Test]
        public async Task DeleteForumCommentWithInvalidId()
        {
            var id = Guid.Parse("5ac6e020-e48e-468a-a94b-a5cccd559cb3");

            var service = serviceProvider.GetService<ICommentServiceAdmin>();

            var isDeleted = await service.DeleteForumComment(id);

            Assert.That(isDeleted == false);
        }


        [Test]
        public async Task SucceedGetUserForumComments()
        {
            var userId = "TestId";

            var service = serviceProvider.GetService<ICommentServiceAdmin>();

            var userComments = await service.GetUserForumComments(userId);

            var userCommentsList = userComments.ToList();

            Assert.That(userCommentsList.Count == 1);
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

            var recipeComment = new RecipeComment()
            {
                Id = new Guid("33dc9d58-da70-4a37-9978-a9c2ec1abf1d"),
                Author = user,
                AuthorId = user.Id,
                Content = "testContent",
                PublishedOn = DateTime.UtcNow,
                Recipe = recipe,
                RecipeId = recipe.Id,
                IsDeleted = false
            };

            var recipeComment2 = new RecipeComment()
            {
                Id = new Guid("23dc9d58-da70-4a37-9978-a9c2ec1abf1d"),
                Author = user,
                AuthorId = user.Id,
                Content = "testContent",
                PublishedOn = DateTime.UtcNow,
                Recipe = recipe,
                RecipeId = recipe.Id,
                IsDeleted = false
            };

            var postCategory = new PostCategory()
            {
                Id = new Guid("2108683c-f724-4463-bc3c-bdc4da07e5f9"),
                Name = "TestCategory2"
            };

            var forumPost = new ForumPost()
            {
                Id = new Guid("41f031fe-8ac2-481a-96a4-895d7f9a6f79"),
                Author = user,
                AuthorId = user.Id,
                Content = "testContent",
                ImageUrl = "TestImageUrl",
                PublishedOn = DateTime.Now,
                Title = "MyPostTest",
                PostCategory = postCategory,
                PostCategoryId = postCategory.Id,
            };

            var forumComment = new ForumComment()
            {
                ForumPost = forumPost,
                ForumPostId = forumPost.Id,
                Author = user,
                AuthorId = user.Id,
                Content = "TestCommentContent",
                Id = new Guid("5ac6e020-e48e-468a-a94b-a5cccd559cbb"),
                PublishedOn = DateTime.Now,
            };

            await repo.AddAsync(category);
            await repo.AddAsync(dishType);
            await repo.AddAsync(user);
            await repo.AddAsync(recipe);
            await repo.AddAsync(recipeComment);
            await repo.AddAsync(recipeComment2);
            await repo.AddAsync(forumPost);
            await repo.AddAsync(postCategory);
            await repo.AddAsync(forumComment);

            await repo.SaveChangesAsync();
        }
    }
}

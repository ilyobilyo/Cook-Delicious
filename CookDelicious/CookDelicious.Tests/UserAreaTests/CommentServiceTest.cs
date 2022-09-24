using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Comments;
using CookDelicious.Core.Contracts.Forum;
using CookDelicious.Core.Contracts.User;
using CookDelicious.Core.MapProfiles;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Core.Services.Comments;
using CookDelicious.Core.Services.Forum;
using CookDelicious.Core.Services.User;
using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Infrasturcture.Models.Forum;
using CookDelicious.Infrasturcture.Models.Identity;
using CookDelicious.Infrasturcture.Models.Recipes;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace CookDelicious.Tests
{
    public class CommentServiceTest
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
                .AddSingleton<ICommentService, CommentService>()
                .AddSingleton<IForumService, ForumService>()
                .AddSingleton<IUserService, UserService>()
                .AddAutoMapper(typeof(UserMapping),
                                typeof(ForumMapping))
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();

            await SeedAsync(repo);
        }

        [Test]
        public async Task SucceedDeletePostComment()
        {
            var id = Guid.Parse("5ac6e020-e48e-468a-a94b-a5cccd559cbb");

            var service = serviceProvider.GetService<ICommentService>();

            var isDeleted = await service.DeletePostComment(id);

            Assert.That(isDeleted == true);
        }

        [Test]
        public async Task DeletePostCommentWithInvalidId()
        {
            var service = serviceProvider.GetService<ICommentService>();

            var id = Guid.Parse("52c6e020-e48e-468a-a94b-a5cccd559cbb");

            var isDeleted = await service.DeletePostComment(id);

            Assert.That(isDeleted == false);
        }

        [Test]
        public async Task SucceedDeleteRecipeComment()
        {
            var id = Guid.Parse("e246db27-19ad-417d-9f12-ea05c4693586");

            var service = serviceProvider.GetService<ICommentService>();

            var isDeleted = await service.DeleteRecipeComment(id);

            Assert.That(isDeleted == true);
        }

        [Test]
        public async Task DeleteRecipeCommentWithInvalidId()
        {
            var service = serviceProvider.GetService<ICommentService>();

            var id = Guid.Parse("e242db27-19ad-417d-9f12-ea05c4693586");

            var isDeleted = await service.DeleteRecipeComment(id);

            Assert.That(isDeleted == false);
        }

        [Test]
        public async Task SucceedPostRecipeComment()
        {
            var recipeId = Guid.Parse("d86dac3a-e8ca-4205-b99d-fa0d44cfbd74");

            var inputModel = new PostCommentInputModel()
            {
                AuthorName = "TestUsername",
                Content = "testContent"
            };

            var service = serviceProvider.GetService<ICommentService>();

            var error = await service.PostCommentForRecipe(recipeId, inputModel);

            Assert.That(error == null);
        }

        [Test]
        public async Task PostRecipeCommentWithInvalidUsername()
        {
            var recipeId = Guid.Parse("d86dac3a-e8ca-4205-b99d-fa0d44cfbd74");

            var inputModel = new PostCommentInputModel()
            {
                AuthorName = "asdasdasd",
                Content = "testContent"
            };

            var service = serviceProvider.GetService<ICommentService>();

            var error = await service.PostCommentForRecipe(recipeId, inputModel);

            Assert.That(error.Messages == PostsConstants.InvalidRecipeOrUser);
        }

        [Test]
        public async Task PostRecipeCommentWithInvalidRecipeId()
        {
            var recipeId = Guid.Parse("d86d3c3a-e8ca-4205-b99d-fa0d44cfbd74");

            var inputModel = new PostCommentInputModel()
            {
                AuthorName = "TestUsername",
                Content = "testContent"
            };

            var service = serviceProvider.GetService<ICommentService>();

            var error = await service.PostCommentForRecipe(recipeId, inputModel);

            Assert.That(error.Messages == PostsConstants.InvalidRecipeOrUser);
        }

        [Test]
        public async Task PostRecipeCommentWithInvalidContent()
        {
            var recipeId = Guid.Parse("d86dac3a-e8ca-4205-b99d-fa0d44cfbd74");

            var inputModel = new PostCommentInputModel()
            {
                AuthorName = "TestUsername",
                Content = "testContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContent"
            };

            var service = serviceProvider.GetService<ICommentService>();

            var error = await service.PostCommentForRecipe(recipeId, inputModel);

            Assert.That(error.Messages == CommentConstants.CommentContentLength);
        }

        [Test]
        public async Task SucceedPostForumComment()
        {
            var forumId = Guid.Parse("41f031fe-8ac2-481a-96a4-895d7f9a6f79");

            var inputModel = new PostCommentInputModel()
            {
                AuthorName = "TestUsername",
                Content = "testContent"
            };

            var service = serviceProvider.GetService<ICommentService>();

            var error = await service.PostCommentForPost(forumId, inputModel);

            Assert.That(error == null);
        }

        [Test]
        public async Task PostForumCommentWithInvalidUsername()
        {
            var forumId = Guid.Parse("41f031fe-8ac2-481a-96a4-895d7f9a6f79");

            var inputModel = new PostCommentInputModel()
            {
                AuthorName = "asdasdasd",
                Content = "testContent"
            };

            var service = serviceProvider.GetService<ICommentService>();

            var error = await service.PostCommentForPost(forumId, inputModel);

            Assert.That(error.Messages == PostsConstants.InvalidRecipeOrUser);
        }

        [Test]
        public async Task PostForumCommentWithInvalidForumPostId()
        {
            var forumId = Guid.Parse("41f031fe-8ac2-481a-96a4-895d7f9a6f70");

            var inputModel = new PostCommentInputModel()
            {
                AuthorName = "TestUsername",
                Content = "testContent"
            };

            var service = serviceProvider.GetService<ICommentService>();

            var error = await service.PostCommentForPost(forumId, inputModel);

            Assert.That(error.Messages == PostsConstants.InvalidRecipeOrUser);
        }

        [Test]
        public async Task PostForumCommentWithInvalidContent()
        {
            var forumId = Guid.Parse("41f031fe-8ac2-481a-96a4-895d7f9a6f79");

            var inputModel = new PostCommentInputModel()
            {
                AuthorName = "TestUsername",
                Content = "testContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContenttestContent"
            };

            var service = serviceProvider.GetService<ICommentService>();

            var error = await service.PostCommentForPost(forumId, inputModel);

            Assert.That(error.Messages == CommentConstants.CommentContentLength);
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

            var postCategory = new PostCategory()
            {
                Name = "TestPostCategory",
                Id = new Guid("b4ec4f2a-e748-4942-946c-f0b0095f8132"),
            };

            var dishType = new DishType()
            {
                Name = "TestDishType",
            };

            var category = new Category()
            {
                Name = "TestCategory"
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

            var forumComment = new ForumComment()
            {
                Id = new Guid("5ac6e020-e48e-468a-a94b-a5cccd559cbb"),
                ForumPost = forumPost,
                ForumPostId = forumPost.Id,
                Author = user,
                AuthorId = user.Id,
                Content = "TestCommentContent",
                PublishedOn = DateTime.Now,
            };

            var recipeComment = new RecipeComment()
            {
                Id = new Guid("e246db27-19ad-417d-9f12-ea05c4693586"),
                Author = user,
                Content = "testContent",
                PublishedOn = DateTime.UtcNow,
                Recipe = recipe,
                RecipeId = recipe.Id,
                IsDeleted = false
            };

            await repo.AddAsync(user);
            await repo.AddAsync(postCategory);
            await repo.AddAsync(dishType);
            await repo.AddAsync(category);
            await repo.AddAsync(forumPost);
            await repo.AddAsync(recipe);
            await repo.AddAsync(forumComment);
            await repo.AddAsync(recipeComment);

            await repo.SaveChangesAsync();
        }
    }
}

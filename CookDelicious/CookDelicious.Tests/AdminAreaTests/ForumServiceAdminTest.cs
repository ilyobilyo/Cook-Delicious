using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.MapProfiles;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Core.Services.Admin;
using CookDelicious.Infrasturcture.Models.Forum;
using CookDelicious.Infrasturcture.Models.Identity;
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
    public class ForumServiceAdminTest
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
                .AddSingleton<IForumServiceAdmin, ForumServiceAdmin>()
                .AddAutoMapper(typeof(ForumMapping))
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();

            await SeedAsync(repo);
        }

        [Test]
        public async Task SucceedCreatePostCategory()
        {
            var inputModel = new CreatePostCategoryInputModel()
            {
                Name = "Test123"
            };

            var service = serviceProvider.GetService<IForumServiceAdmin>();

            var error = await service.CreatePostCategory(inputModel);

            Assert.That(error == null);
        }

        [Test]
        public async Task CreatePostCategoryThatAlreadyExists()
        {
            var inputModel = new CreatePostCategoryInputModel()
            {
                Name = "TestCategory2"
            };

            var service = serviceProvider.GetService<IForumServiceAdmin>();

            var error = await service.CreatePostCategory(inputModel);

            Assert.That(error.Messages == $"{inputModel.Name} {MessageConstant.AlreadyExist}");
        }

        [Test]
        public async Task SucceedDeleteForumPostCategory()
        {
            var id = Guid.Parse("2108683c-f724-4463-bc3c-bdc4da07e5f9");

            var service = serviceProvider.GetService<IForumServiceAdmin>();

            var isDeleted = await service.DeleteForumPostCategory(id);

            Assert.That(isDeleted == true);
        }

        [Test]
        public async Task DeleteForumPostCategoryWithInvalidId()
        {
            var id = Guid.Parse("2108683c-f724-4463-bc3c-bdc4da07e3f9");

            var service = serviceProvider.GetService<IForumServiceAdmin>();

            var isDeleted = await service.DeleteForumPostCategory(id);

            Assert.That(isDeleted == false);
        }

        [Test]
        public async Task SucceedDeleteForumPost()
        {
            var id = Guid.Parse("41f031fe-8ac2-481a-96a4-895d7f9a6f79");

            var service = serviceProvider.GetService<IForumServiceAdmin>();

            var isDeleted = await service.DeletePost(id);

            Assert.That(isDeleted == true);
        }

        [Test]
        public async Task DeleteForumPostWithInvalidId()
        {
            var id = Guid.Parse("41f031fe-8ac2-481a-96a4-895d739a6f79");

            var service = serviceProvider.GetService<IForumServiceAdmin>();

            var isDeleted = await service.DeletePost(id);

            Assert.That(isDeleted == false);
        }

        [Test]
        public async Task SucceedGetAllNotDeletedForumPostCategories()
        {
            var service = serviceProvider.GetService<IForumServiceAdmin>();

            var allCategories = await service.GetAllForumPostCategories();

            var allCategoriesList = allCategories.ToList();

            Assert.That(allCategoriesList.Count == 1);
        }

        [Test]
        public async Task SucceedGetAllNotDeletedUserPosts()
        {
            var service = serviceProvider.GetService<IForumServiceAdmin>();

            var userId = "TestId";

            var allUserPosts = await service.GetAllUserPosts(userId);

            var allUserPostsList = allUserPosts.ToList();

            Assert.That(allUserPostsList.Count == 1);
        }

        [Test]
        public async Task SucceedGetAllUsersForPageing()
        {
            var service = serviceProvider.GetService<IForumServiceAdmin>();

            var userId = "TestId";

            var allUserPosts = await service.GetAllUserPosts(userId);

            var allUserPostsList = allUserPosts.ToList();

            Assert.That(allUserPostsList.Count == 1);
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
                Id = new Guid("2108683c-f724-4463-bc3c-bdc4da07e5f9"),
                Name = "TestCategory2"
            };

            var postCategory2 = new PostCategory()
            {
                Id = new Guid("3108683c-f724-4463-bc3c-bdc4da07e5f9"),
                Name = "TestCategory",
                IsDeleted = true
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

            var forumPost2 = new ForumPost()
            {
                Id = new Guid("42f031fe-8ac2-481a-96a4-895d7f9a6f79"),
                Author = user,
                AuthorId = user.Id,
                Content = "testContent",
                ImageUrl = "TestImageUrl",
                PublishedOn = DateTime.Now,
                Title = "MyPostTest",
                PostCategory = postCategory,
                PostCategoryId = postCategory.Id,
                IsDeleted = true
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

            await repo.AddAsync(user);
            await repo.AddAsync(forumPost);
            await repo.AddAsync(forumPost2);
            await repo.AddAsync(postCategory);
            await repo.AddAsync(postCategory2);
            await repo.AddAsync(forumComment);

            await repo.SaveChangesAsync();
        }
    }
}

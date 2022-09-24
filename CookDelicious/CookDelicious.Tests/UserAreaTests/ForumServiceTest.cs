using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Forum;
using CookDelicious.Core.Contracts.User;
using CookDelicious.Core.MapProfiles;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Core.Services.Forum;
using CookDelicious.Core.Services.User;
using CookDelicious.Infrasturcture.Models.Forum;
using CookDelicious.Infrasturcture.Models.Identity;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CookDelicious.Tests
{
    public class ForumServiceTest
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
                .AddSingleton<IForumService, ForumService>()
                .AddSingleton<IUserService, UserService>()
                .AddAutoMapper(typeof(UserMapping),
                                typeof(ForumMapping))
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();

            await SeedAsync(repo);
        }

        [Test]
        public async Task SucceedGetForumPostById()
        {
            var service = serviceProvider.GetService<IForumService>();

            var id = Guid.Parse("41f031fe-8ac2-481a-96a4-895d7f9a6f79");

            var post = await service.GetById(id);

            Assert.That(post != null);
        }

        [Test]
        public async Task GetForumPostByIdWithInvalidId()
        {
            var service = serviceProvider.GetService<IForumService>();

            var id = Guid.Parse("41f031fe-8ac2-481a-96a4-895d7f9a6f19");

            var post = await service.GetById(id);

            Assert.That(post == null);
        }

        [Test]
        public async Task SucceedCreateForumPost()
        {
            var service = serviceProvider.GetService<IForumService>();

            var username = "TestUsername";

            var model = new CreateForumPostInputModel()
            {
                AuthorId = "TestId",
                Category = "TestPostCategory",
                Description = "Test Description",
                ImageUrl = "TestUrl",
                Title = "Test post",
                PublishedOn = DateTime.Now
            };

            var error = await service.CreatePost(model, username);

            Assert.That(error == null);
        }

        [Test]
        public async Task CreateForumPostWithInvalidTitle()
        {
            var service = serviceProvider.GetService<IForumService>();

            var username = "TestUsername";

            var model = new CreateForumPostInputModel()
            {
                AuthorId = "TestId",
                Category = "TestPostCategory",
                Description = "Test Description",
                ImageUrl = "TestUrl",
                Title = null,
                PublishedOn = DateTime.Now
            };

            var error = await service.CreatePost(model, username);

            Assert.That(error.Messages == PostsConstants.RequiredTitleAndDescription);
        }

        [Test]
        public async Task CreateForumPostWithInvalidDescription()
        {
            var service = serviceProvider.GetService<IForumService>();

            var username = "TestUsername";

            var model = new CreateForumPostInputModel()
            {
                AuthorId = "TestId",
                Category = "TestPostCategory",
                Description = null,
                ImageUrl = "TestUrl",
                Title = "Test post",
                PublishedOn = DateTime.Now
            };

            var error = await service.CreatePost(model, username);

            Assert.That(error.Messages == PostsConstants.RequiredTitleAndDescription);
        }

        [Test]
        public async Task CreateForumPostWithInvalidAuthor()
        {
            var service = serviceProvider.GetService<IForumService>();

            var username = "asdasdasd";

            var model = new CreateForumPostInputModel()
            {
                Category = "TestPostCategory",
                Description = "desc",
                ImageUrl = "TestUrl",
                Title = "Test post",
                PublishedOn = DateTime.Now
            };

            var error = await service.CreatePost(model, username);

            Assert.That(error.Messages == UserConstants.InvalidAuthor);
        }

        [Test]
        public async Task SucceedDeleteForumPost()
        {
            var service = serviceProvider.GetService<IForumService>();

            var id = Guid.Parse("41f031fe-8ac2-481a-96a4-895d7f9a6f79");

            var isDeleted = await service.DeletePost(id);

            Assert.That(isDeleted == true);
        }

        [Test]
        public async Task DeleteForumPostWithInvalidId()
        {
            var service = serviceProvider.GetService<IForumService>();

            var id = Guid.Parse("41f031fe-8ac2-481a-96a4-895d7f3a6f79");

            var isDeleted = await service.DeletePost(id);

            Assert.That(isDeleted == false);
        }

        [Test]
        public async Task SucceedGetAllCategoryNames()
        {
            var service = serviceProvider.GetService<IForumService>();

            var categories = await service.GetAllPostCategoryNames();

            Assert.That(categories.Count > 0);
        }

        [Test]
        public async Task GetSortedPostsWithValidParameters()
        {
            var service = serviceProvider.GetService<IForumService>();

            string sortCategory = "TestPostCategory";

            var posts = await service.GetAllSortPostsForPageing(1, 6, sortCategory);

            Assert.That(posts.Items.Count() > 0 && posts.TotalCount > 0);
        }

        [Test]
        public async Task GetSortedPostsWithInvalidParameters()
        {
            var service = serviceProvider.GetService<IForumService>();

            string sortCategory = "asd";

            var posts = await service.GetAllSortPostsForPageing(1, 6, sortCategory);

            Assert.That(posts.Items.Count() == 0 && posts.TotalCount == 0);
        }

        [Test]
        public async Task SucceedGetArchive()
        {
            var service = serviceProvider.GetService<IForumService>();

            var archive = await service.GetArchive();

            Assert.That(archive.Count > 0);
        }

        [Test]
        public async Task SucceedGetPostServiceModelById()
        {
            var service = serviceProvider.GetService<IForumService>();

            var id = Guid.Parse("41f031fe-8ac2-481a-96a4-895d7f9a6f79");

            var post = await service.GetPostServiceModelById(id);

            Assert.That(post != null);
        }

        [Test]
        public async Task GetPostServiceModelByIdWithinvalidId()
        {
            var service = serviceProvider.GetService<IForumService>();

            var id = Guid.Parse("41f031fe-8ac2-481a-96a4-892d7f9a6f79");

            var post = await service.GetPostServiceModelById(id);

            Assert.That(post == null);
        }

        [Test]
        public async Task SucceedGetPostComments()
        {
            var service = serviceProvider.GetService<IForumService>();

            var id = Guid.Parse("41f031fe-8ac2-481a-96a4-895d7f9a6f79");

            var comments = await service.GetCommentsPerPage(id, 1, 5);

            var commentsList = comments.ToList();

            Assert.That(commentsList.Count > 0);
        }

        [Test]
        public async Task GetPostCommentsWithInvalidId()
        {
            var service = serviceProvider.GetService<IForumService>();

            var id = Guid.Parse("41f031fe-8ac2-481a-91a4-895d7f9a6f79");

            var comments = await service.GetCommentsPerPage(id, 1, 5);

            var commentsList = comments.ToList();

            Assert.That(commentsList.Count == 0);
        }

        [Test]
        public async Task TestPageingForPosts()
        {
            var service = serviceProvider.GetService<IForumService>();

            string sortCategory = "TestPostCategory";

            var posts = await service.GetAllSortPostsForPageing(1, 5, sortCategory);

            Assert.That(posts.Items.Count() == 5 && posts.TotalCount == 6);
        }

        [Test]
        public async Task TestPageingForPostsComments()
        {
            var service = serviceProvider.GetService<IForumService>();

            var id = Guid.Parse("41f031fe-8ac2-481a-96a4-895d7f9a6f79");

            var comments = await service.GetCommentsPerPage(id, 1, 5);

            var commentsList = comments.ToList();

            Assert.That(commentsList.Count == 5);
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

            var category = new PostCategory()
            {
                Name = "TestPostCategory",
                Id = new Guid("b4ec4f2a-e748-4942-946c-f0b0095f8132"),
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
                PostCategory = category,
                PostCategoryId = category.Id,
            };

            var forumPost2 = new ForumPost()
            {
                Id = new Guid("21f031fe-8ac2-481a-96a4-895d7f9a6f79"),
                Author = user,
                AuthorId = user.Id,
                Content = "testContent",
                ImageUrl = "TestImageUrl",
                PublishedOn = DateTime.Now,
                Title = "MyPostTest",
                PostCategory = category,
                PostCategoryId = category.Id,
            };

            var forumPost3 = new ForumPost()
            {
                Id = new Guid("31f031fe-8ac2-481a-96a4-895d7f9a6f79"),
                Author = user,
                AuthorId = user.Id,
                Content = "testContent",
                ImageUrl = "TestImageUrl",
                PublishedOn = DateTime.Now,
                Title = "MyPostTest",
                PostCategory = category,
                PostCategoryId = category.Id,
            };

            var forumPost4 = new ForumPost()
            {
                Id = new Guid("51f031fe-8ac2-481a-96a4-895d7f9a6f79"),
                Author = user,
                AuthorId = user.Id,
                Content = "testContent",
                ImageUrl = "TestImageUrl",
                PublishedOn = DateTime.Now,
                Title = "MyPostTest",
                PostCategory = category,
                PostCategoryId = category.Id,
            };

            var forumPost5 = new ForumPost()
            {
                Id = new Guid("61f031fe-8ac2-481a-96a4-895d7f9a6f79"),
                Author = user,
                AuthorId = user.Id,
                Content = "testContent",
                ImageUrl = "TestImageUrl",
                PublishedOn = DateTime.Now,
                Title = "MyPostTest",
                PostCategory = category,
                PostCategoryId = category.Id,
            };

            var forumPost6 = new ForumPost()
            {
                Id = new Guid("71f031fe-8ac2-481a-96a4-895d7f9a6f79"),
                Author = user,
                AuthorId = user.Id,
                Content = "testContent",
                ImageUrl = "TestImageUrl",
                PublishedOn = DateTime.Now,
                Title = "MyPostTest",
                PostCategory = category,
                PostCategoryId = category.Id,
            };

            var forumComment = new ForumComment()
            {
                Author = user,
                AuthorId = user.Id,
                Content = "TestCommentContent",
                Id = new Guid("5ac6e020-e48e-468a-a94b-a5cccd559cbb"),
                PublishedOn = DateTime.Now,
            };

            var forumComment2 = new ForumComment()
            {
                Author = user,
                AuthorId = user.Id,
                Content = "TestCommentContent",
                Id = new Guid("5ac6e020-e48e-468a-a94b-a5cccd559cb2"),
                PublishedOn = DateTime.Now,
            };

            var forumComment3 = new ForumComment()
            {
                Author = user,
                AuthorId = user.Id,
                Content = "TestCommentContent",
                Id = new Guid("5ac6e020-e48e-468a-a94b-a5cccd559cb3"),
                PublishedOn = DateTime.Now,
            };

            var forumComment4 = new ForumComment()
            {
                Author = user,
                AuthorId = user.Id,
                Content = "TestCommentContent",
                Id = new Guid("5ac6e020-e48e-468a-a94b-a5cccd559cb4"),
                PublishedOn = DateTime.Now,
            };

            var forumComment5 = new ForumComment()
            {
                Author = user,
                AuthorId = user.Id,
                Content = "TestCommentContent",
                Id = new Guid("5ac6e020-e48e-468a-a94b-a5cccd559cb5"),
                PublishedOn = DateTime.Now,
            };

            var forumComment6 = new ForumComment()
            {
                Author = user,
                AuthorId = user.Id,
                Content = "TestCommentContent",
                Id = new Guid("5ac6e020-e48e-468a-a94b-a5cccd559cb6"),
                PublishedOn = DateTime.Now,
            };

            forumPost.ForumComments.Add(forumComment);
            forumPost.ForumComments.Add(forumComment2);
            forumPost.ForumComments.Add(forumComment3);
            forumPost.ForumComments.Add(forumComment4);
            forumPost.ForumComments.Add(forumComment5);
            forumPost.ForumComments.Add(forumComment6);


            await repo.AddAsync(forumPost);
            await repo.AddAsync(forumPost2);
            await repo.AddAsync(forumPost3);
            await repo.AddAsync(forumPost4);
            await repo.AddAsync(forumPost5);
            await repo.AddAsync(forumPost6);
            await repo.AddAsync(user);
            await repo.AddAsync(category);
            await repo.AddAsync(forumComment);
            await repo.AddAsync(forumComment2);
            await repo.AddAsync(forumComment3);
            await repo.AddAsync(forumComment4);
            await repo.AddAsync(forumComment5);
            await repo.AddAsync(forumComment6);

            await repo.SaveChangesAsync();
        }
    }
}

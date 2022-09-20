using CookDelicious.Core.Contracts.Blog;
using CookDelicious.Core.MapProfiles;
using CookDelicious.Core.Services.BlogService;
using CookDelicious.Infrasturcture.Models.Blog;
using CookDelicious.Infrasturcture.Models.Identity;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CookDelicious.Tests
{
    public class BlogServiceTest
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
                .AddSingleton<IBlogService, BlogService>()
                .AddAutoMapper(typeof(UserMapping),
                                typeof(BlogMapping))
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();

            await SeedAsync(repo);
        }

        [Test]
        public async Task SucceedGetAllSortBlogPostsForPageingWithPostCategory()
        {
            int pageNumber = 1;
            int pageSize = 2;
            string blogPostCategory = "TestCategory";
            int? sortMonth = null;

            var service = serviceProvider.GetService<IBlogService>();

            (var post, var totalCount) = await service.GetAllSortBlogPostsForPageing(pageNumber, pageSize, blogPostCategory, sortMonth);

            var postList = post.ToList();

            Assert.That(postList.Count == 2 && totalCount == 3);
        }

        [Test]
        public async Task SucceedGetAllSortBlogPostsForPageingWithMonth()
        {
            int pageNumber = 1;
            int pageSize = 2;
            string blogPostCategory = null;
            int? sortMonth = 9;

            var service = serviceProvider.GetService<IBlogService>();

            (var post, var totalCount) = await service.GetAllSortBlogPostsForPageing(pageNumber, pageSize, blogPostCategory, sortMonth);

            var postList = post.ToList();

            Assert.That(postList.Count == 2 && totalCount == 3);
        }

        [Test]
        public async Task SucceedGetAllSortBlogPostsForPageingWithoutSortParameters()
        {
            int pageNumber = 1;
            int pageSize = 2;
            string blogPostCategory = null;
            int? sortMonth = null;

            var service = serviceProvider.GetService<IBlogService>();

            (var post, var totalCount) = await service.GetAllSortBlogPostsForPageing(pageNumber, pageSize, blogPostCategory, sortMonth);

            var postList = post.ToList();

            Assert.That(postList.Count == 2 && totalCount == 6);
        }

        [Test]
        public async Task SucceedGetAllBlogPostCategoryNames()
        {
            var service = serviceProvider.GetService<IBlogService>();

            var postCategories = await service.GetBlogAllPostCategoryNames();

            Assert.That(postCategories.Count == 2);
        }

        [Test]
        public async Task SucceedGetAllBlogArchive()
        {
            var service = serviceProvider.GetService<IBlogService>();

            var archive = await service.GetBlogArchive();

            Assert.That(archive.Count == 2);
        }

        [Test]
        public async Task SucceedGetBlogPostById()
        {
            var service = serviceProvider.GetService<IBlogService>();

            var id = Guid.Parse("c982140f-a7b3-4f4e-9957-4425bb91756d");

            var post = await service.GetBlogPostServiceModelById(id);

            Assert.That(post != null);
        }

        [Test]
        public async Task GetBlogPostByIdWithInvalidId()
        {
            var service = serviceProvider.GetService<IBlogService>();

            var id = Guid.Parse("c982140f-a7b3-4f4e-9957-4125bb91756d");

            var post = await service.GetBlogPostServiceModelById(id);

            Assert.That(post == null);
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

            var category = new BlogPostCategory()
            {
                Name = "TestCategory"
            };

            var category2 = new BlogPostCategory()
            {
                Name = "TestCategory2"
            };

            var blogPost = new BlogPost()
            {
                Author = user,
                AuthorId = user.Id,
                BlogPostCategory = category,
                BlogPostCategoryId = category.Id,
                Content = "TestContent",
                Id = new Guid("c982140f-a7b3-4f4e-9957-4425bb91756d"),
                ImageUrl = "TestUrl",
                PublishedOn = DateTime.Now.AddMonths(1),
                Title = "Title"
            };

            var blogPost2 = new BlogPost()
            {
                Author = user,
                AuthorId = user.Id,
                BlogPostCategory = category,
                BlogPostCategoryId = category.Id,
                Content = "TestContent",
                Id = new Guid("c982140f-a7b3-4f4e-9957-4425bb917562"),
                ImageUrl = "TestUrl",
                PublishedOn = DateTime.Now.AddMonths(1),
                Title = "Title"
            };

            var blogPost3 = new BlogPost()
            {
                Author = user,
                AuthorId = user.Id,
                BlogPostCategory = category,
                BlogPostCategoryId = category.Id,
                Content = "TestContent2",
                Id = new Guid("c982140f-a7b3-4f4e-9957-4425bb917563"),
                ImageUrl = "TestUrl",
                PublishedOn = DateTime.Now.AddMonths(1),
                Title = "Title"
            };

            var blogPost4 = new BlogPost()
            {
                Author = user,
                AuthorId = user.Id,
                BlogPostCategory = category2,
                BlogPostCategoryId = category2.Id,
                Content = "TestContent2",
                Id = new Guid("c982140f-a7b3-4f4e-9957-4425bb917564"),
                ImageUrl = "TestUrl",
                PublishedOn = DateTime.Now,
                Title = "Title"
            };

            var blogPost5 = new BlogPost()
            {
                Author = user,
                AuthorId = user.Id,
                BlogPostCategory = category2,
                BlogPostCategoryId = category2.Id,
                Content = "TestContent2",
                Id = new Guid("c982140f-a7b3-4f4e-9957-4425bb917565"),
                ImageUrl = "TestUrl",
                PublishedOn = DateTime.Now,
                Title = "Title"
            };

            var blogPost6 = new BlogPost()
            {
                Author = user,
                AuthorId = user.Id,
                BlogPostCategory = category2,
                BlogPostCategoryId = category2.Id,
                Content = "TestContent",
                Id = new Guid("c982140f-a7b3-4f4e-9957-4425bb917566"),
                ImageUrl = "TestUrl",
                PublishedOn = DateTime.Now,
                Title = "Title"
            };

            await repo.AddAsync(user);
            await repo.AddAsync(category);
            await repo.AddAsync(category2);
            await repo.AddAsync(blogPost);
            await repo.AddAsync(blogPost2);
            await repo.AddAsync(blogPost3);
            await repo.AddAsync(blogPost4);
            await repo.AddAsync(blogPost5);
            await repo.AddAsync(blogPost6);
            await repo.SaveChangesAsync();
        }
    }
}

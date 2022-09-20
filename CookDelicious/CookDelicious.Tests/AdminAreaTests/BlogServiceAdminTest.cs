using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Contracts.User;
using CookDelicious.Core.MapProfiles;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Core.Services.Admin;
using CookDelicious.Core.Services.User;
using CookDelicious.Infrasturcture.Models.Blog;
using CookDelicious.Infrasturcture.Models.Identity;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CookDelicious.Tests.AdminAreaTests
{
    public class BlogServiceAdminTest
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
                .AddSingleton<IBlogServiceAdmin, BlogServiceAdmin>()
                .AddSingleton<IUserService, UserService>()
                .AddAutoMapper(typeof(BlogMapping))
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();

            await SeedAsync(repo);
        }

        [Test]
        public async Task SucceedCreateBlogPost()
        {
            var inputModel = new CreateBlogPostInputModel()
            {
                Category = "TestCategory",
                Description = "TestDescription",
                ImageUrl = "TestUrl",
                PublishedOn = DateTime.Now,
                Title = "title"
            };

            var username = "TestUsername";

            var service = serviceProvider.GetService<IBlogServiceAdmin>();

            var error = await service.CreateBlogPost(inputModel, username);

            Assert.That(error == null);
        }

        [Test]
        public async Task CreateBlogPostWithInvalidTitle()
        {
            var inputModel = new CreateBlogPostInputModel()
            {
                Category = "TestCategory",
                Description = "TestDescription",
                ImageUrl = "TestUrl",
                PublishedOn = DateTime.Now,
                Title = null
            };

            var username = "TestUsername";

            var service = serviceProvider.GetService<IBlogServiceAdmin>();

            var error = await service.CreateBlogPost(inputModel, username);

            Assert.That(error.Messages == PostsConstants.RequiredTitleAndContent);
        }

        [Test]
        public async Task CreateBlogPostWithInvalidDescription()
        {
            var inputModel = new CreateBlogPostInputModel()
            {
                Category = "TestCategory",
                Description = null,
                ImageUrl = "TestUrl",
                PublishedOn = DateTime.Now,
                Title = "title"
            };

            var username = "TestUsername";

            var service = serviceProvider.GetService<IBlogServiceAdmin>();

            var error = await service.CreateBlogPost(inputModel, username);

            Assert.That(error.Messages == PostsConstants.RequiredTitleAndContent);
        }

        [Test]
        public async Task CreateBlogPostWithInvalidUsername()
        {
            var inputModel = new CreateBlogPostInputModel()
            {
                Category = "TestCategory",
                Description = "TestDescription",
                ImageUrl = "TestUrl",
                PublishedOn = DateTime.Now,
                Title = "title"
            };

            var username = "asdasdasdas";

            var service = serviceProvider.GetService<IBlogServiceAdmin>();

            var error = await service.CreateBlogPost(inputModel, username);

            Assert.That(error.Messages == UserConstants.InvalidAuthor);
        }

        [Test]
        public async Task SucceedCreateBlogPostCategory()
        {
            var inputModel = new CreateBlogPostCategoryInputModel()
            {
                Name = "Test123"
            };

            var service = serviceProvider.GetService<IBlogServiceAdmin>();

            var error = await service.CreateBlogPostCategory(inputModel);

            Assert.That(error == null);
        }

        [Test]
        public async Task CreateBlogPostCategoryWhoesAlreadyExists()
        {
            var inputModel = new CreateBlogPostCategoryInputModel()
            {
                Name = "TestCategory"
            };

            var service = serviceProvider.GetService<IBlogServiceAdmin>();

            var error = await service.CreateBlogPostCategory(inputModel);

            Assert.That(error.Messages == $"{inputModel.Name} {MessageConstant.AlreadyExist}");
        }

        [Test]
        public async Task SucceedDeleteBlogPost()
        {
            var service = serviceProvider.GetService<IBlogServiceAdmin>();

            var id = Guid.Parse("c982140f-a7b3-4f4e-9957-4425bb91756d");

            var isDeleted = await service.DeleteBlogPost(id);

            Assert.That(isDeleted == true);
        }

        [Test]
        public async Task DeleteBlogPostWithInvalidId()
        {
            var service = serviceProvider.GetService<IBlogServiceAdmin>();

            var id = Guid.Parse("c912140f-a7b3-4f4e-9957-4425bb91756d");

            var isDeleted = await service.DeleteBlogPost(id);

            Assert.That(isDeleted == false);
        }

        [Test]
        public async Task SucceedDeleteBlogPostCategory()
        {
            var service = serviceProvider.GetService<IBlogServiceAdmin>();

            var id = Guid.Parse("06e3cdd6-a590-44cc-8861-816cf139d7db");

            var isDeleted = await service.DeleteBlogPostCategory(id);

            Assert.That(isDeleted == true);
        }

        [Test]
        public async Task DeleteBlogPostCategoryWithInvalidId()
        {
            var service = serviceProvider.GetService<IBlogServiceAdmin>();

            var id = Guid.Parse("06e3cdd6-a590-44cc-8861-816c2139d7db");

            var isDeleted = await service.DeleteBlogPostCategory(id);

            Assert.That(isDeleted == false);
        }

        [Test]
        public async Task SucceedGetAllBlogPostCategories()
        {
            var service = serviceProvider.GetService<IBlogServiceAdmin>();

            var allCategories = await service.GetAllBlogPostCategories();

            var categoryList = allCategories.ToList();

            Assert.That(categoryList.Count == 2);
        }

        [Test]
        public async Task SucceedGetAllBlogPostCategoryNames()
        {
            var service = serviceProvider.GetService<IBlogServiceAdmin>();

            var allCategories = await service.GetAllBlogPostCategoryNames();

            var categoryNamesList = allCategories.ToList();

            Assert.That(categoryNamesList.Count == 2);
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
                Id = new Guid("06e3cdd6-a590-44cc-8861-816cf139d7db"),
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

            await repo.AddAsync(user);
            await repo.AddAsync(category);
            await repo.AddAsync(category2);
            await repo.AddAsync(blogPost);
            await repo.AddAsync(blogPost2);
            await repo.SaveChangesAsync();
        }
    }
}

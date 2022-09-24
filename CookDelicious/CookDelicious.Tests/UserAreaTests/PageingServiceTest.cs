using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Blog;
using CookDelicious.Core.Contracts.Common.Categories;
using CookDelicious.Core.Contracts.Common.DishTypes;
using CookDelicious.Core.Contracts.Forum;
using CookDelicious.Core.Contracts.Pageing;
using CookDelicious.Core.Contracts.Product;
using CookDelicious.Core.Contracts.Recipes;
using CookDelicious.Core.Contracts.User;
using CookDelicious.Core.MapProfiles;
using CookDelicious.Core.Models.Sorting;
using CookDelicious.Core.Services.BlogService;
using CookDelicious.Core.Services.Common.Categories;
using CookDelicious.Core.Services.Common.DishTypes;
using CookDelicious.Core.Services.Forum;
using CookDelicious.Core.Services.Pageing;
using CookDelicious.Core.Services.Products;
using CookDelicious.Core.Services.Recipes;
using CookDelicious.Core.Services.User;
using CookDelicious.Infrasturcture.Models.Blog;
using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Infrasturcture.Models.Forum;
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

namespace CookDelicious.Tests.UserAreaTests
{
    public class PageingServiceTest
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
                .AddSingleton<IPageingService, PageingService>()
                .AddSingleton<IBlogService, BlogService>()
                .AddSingleton<IForumService, ForumService>()
                .AddSingleton<IRecipeService, RecipeService>()
                .AddSingleton<IProductService, ProductService>()
                .AddSingleton<ICategoryService, CategoryService>()
                .AddSingleton<IUserService, UserService>()
                .AddSingleton<IDishTypeService, DishTypeService>()
                .AddAutoMapper(typeof(UserMapping),
                                typeof(ForumMapping),
                                typeof(BlogMapping),
                                typeof(CategoryMapping),
                                typeof(RecipeMapping),
                                typeof(ProductMapping)
                                )
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();

            await SeedAsync(repo);
        }

        [Test]
        public async Task CheckGetBlogHomePagedModelWithoutSortParamsPostsCount()
        {
            int pageNumber = 1;

            var service = serviceProvider.GetService<IPageingService>();

            var list = await service.GetBlogHomePagedModel(pageNumber);

            Assert.That(list.Posts.Count == 2);
        }

        [Test]
        public async Task CheckGetBlogHomePagedModelWithoutSortParamsCategoriesCount()
        {
            int pageNumber = 1;

            var service = serviceProvider.GetService<IPageingService>();

            var list = await service.GetBlogHomePagedModel(pageNumber);

            Assert.That(list.Categories.Count == 1);
        }

        [Test]
        public async Task CheckGetBlogHomePagedModelWithoutSortParamsArchivesCount()
        {
            int pageNumber = 1;

            var service = serviceProvider.GetService<IPageingService>();

            var list = await service.GetBlogHomePagedModel(pageNumber);

            Assert.That(list.Archive.Count == 1);
        }

        [Test]
        public async Task CheckGetBlogHomePagedModelWithSortingParams()
        {
            int pageNumber = 1;

            var sortCategory = "TestCategory2";

            var service = serviceProvider.GetService<IPageingService>();

            var list = await service.GetBlogHomePagedModel(pageNumber, sortCategory);

            Assert.That(list.Sorting.Category == "TestCategory2");
        }

        [Test]
        public async Task CheckGetForumHomePagedModelWithoutSortParamsPostsCount()
        {
            int pageNumber = 1;

            var service = serviceProvider.GetService<IPageingService>();

            var list = await service.GetForumHomePagedModel(pageNumber);

            Assert.That(list.Posts.Count == 2);
        }

        [Test]
        public async Task CheckGetForumHomePagedModelWithoutSortParamsCategoriesCount()
        {
            int pageNumber = 1;

            var service = serviceProvider.GetService<IPageingService>();

            var list = await service.GetForumHomePagedModel(pageNumber);

            Assert.That(list.Categories.Count == 1);
        }

        [Test]
        public async Task CheckGetForumHomePagedModelWithoutSortParamsArchivesCount()
        {
            int pageNumber = 1;

            var service = serviceProvider.GetService<IPageingService>();

            var list = await service.GetForumHomePagedModel(pageNumber);

            Assert.That(list.Archive.Count == 1);
        }

        [Test]
        public async Task CheckGetForumHomePagedModelWithSortingParams()
        {
            int pageNumber = 1;

            var sortCategory = "TestPostCategory";

            var service = serviceProvider.GetService<IPageingService>();

            var list = await service.GetForumHomePagedModel(pageNumber, sortCategory);

            Assert.That(list.Sorting.Category == "TestPostCategory");
        }


        [Test]
        public async Task SucceedGetForumPostPagedModel()
        {
            int pageNumber = 1;

            var id = Guid.Parse("41f031fe-8ac2-481a-96a4-895d7f9a6f79");

            var service = serviceProvider.GetService<IPageingService>();

            var post = await service.GetForumPostPagedModel(id, pageNumber);

            Assert.That(post != null);
        }

        [Test]
        public async Task CheckGetForumPostPagedModelCommentsCount()
        {
            int pageNumber = 1;

            var id = Guid.Parse("41f031fe-8ac2-481a-96a4-895d7f9a6f79");

            var service = serviceProvider.GetService<IPageingService>();

            var post = await service.GetForumPostPagedModel(id,pageNumber);

            Assert.That(post.Comments.Count == 2);
        }

        [Test]
        public async Task SucceedGetProductsPagedModel()
        {
            int pageNumber = 1;

            var service = serviceProvider.GetService<IPageingService>();

            var list = await service.GetProductsPagedModel(pageNumber);

            Assert.That(list.Count == 2);
        }




        private async Task SeedAsync(IApplicationDbRepository repo)
        {
            var dishType = new DishType()
            {
                Name = "TestDishType",
            };

            var category = new Category()
            {
                Name = "TestCategory"
            };

            var product1 = new Product()
            {
                Name = "TestProduct1",
                Type = "test",
                ImageUrl = "TestUrl",
                Description = "TestDesc",
            };

            var product2 = new Product()
            {
                Name = "TestProduct2",
                Type = "test",
                ImageUrl = "TestUrl",
                Description = "TestDesc",
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

            var recipe2 = new Recipe()
            {
                Id = new Guid("d86dac3a-e8ca-4205-b99d-fa0d44cfbd75"),
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

           

            var recipeProducts = new List<RecipeProduct>()
            {
                new RecipeProduct
                {
                    Recipe = recipe,
                    Quantity = "20",
                    Product = product1
                },

            };

            recipe.RecipeProducts = recipeProducts;

            var recipeComment = new RecipeComment()
            {
                Author = user,
                Content = "testContent",
                PublishedOn = DateTime.UtcNow,
                Recipe = recipe,
                RecipeId = recipe.Id,
                IsDeleted = false
            };

            var recipeComment2 = new RecipeComment()
            {
                Author = user,
                Content = "testContent",
                PublishedOn = DateTime.UtcNow,
                Recipe = recipe,
                RecipeId = recipe.Id,
                IsDeleted = false
            };

            var category2 = new BlogPostCategory()
            {
                Name = "TestCategory2"
            };

            var blogPost = new BlogPost()
            {
                Author = user,
                AuthorId = user.Id,
                BlogPostCategory = category2,
                BlogPostCategoryId = category2.Id,
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
                BlogPostCategory = category2,
                BlogPostCategoryId = category2.Id,
                Content = "TestContent",
                Id = new Guid("c982140f-a7b3-4f4e-9957-4425bb917562"),
                ImageUrl = "TestUrl",
                PublishedOn = DateTime.Now.AddMonths(1),
                Title = "Title"
            };

            var categoryForum = new PostCategory()
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
                PostCategory = categoryForum,
                PostCategoryId = categoryForum.Id,
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
                PostCategory = categoryForum,
                PostCategoryId = categoryForum.Id,
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

            var forumComment2 = new ForumComment()
            {
                ForumPost = forumPost,
                ForumPostId = forumPost.Id,
                Author = user,
                AuthorId = user.Id,
                Content = "TestCommentContent",
                Id = new Guid("5ac6e020-e48e-468a-a94b-a5cccd559cb2"),
                PublishedOn = DateTime.Now,
            };


            recipe2.Comments.Add(recipeComment);
            recipe2.Comments.Add(recipeComment2);

            await repo.AddAsync(user);
            await repo.AddAsync(dishType);
            await repo.AddAsync(category);
            await repo.AddAsync(product1);
            await repo.AddAsync(product2);
            await repo.AddAsync(recipe);
            await repo.AddAsync(recipe2);
            await repo.AddAsync(recipeComment);
            await repo.AddAsync(recipeComment2);
            await repo.AddAsync(category2);
            await repo.AddAsync(blogPost);
            await repo.AddAsync(blogPost2);
            await repo.AddAsync(categoryForum);
            await repo.AddAsync(forumPost);
            await repo.AddAsync(forumPost2);
            await repo.AddAsync(forumComment);
            await repo.AddAsync(forumComment2);



            await repo.SaveChangesAsync();
        }
    }
}

using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Common.Categories;
using CookDelicious.Core.Contracts.Common.DishTypes;
using CookDelicious.Core.Contracts.Product;
using CookDelicious.Core.Contracts.Recipes;
using CookDelicious.Core.MapProfiles;
using CookDelicious.Core.Models.Recipe;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Core.Services.Common.Categories;
using CookDelicious.Core.Services.Common.DishTypes;
using CookDelicious.Core.Services.Products;
using CookDelicious.Core.Services.Recipes;
using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Infrasturcture.Models.Identity;
using CookDelicious.Infrasturcture.Models.Recipes;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookDelicious.Tests
{
    public class Tests
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
                .AddSingleton<IRecipeService, RecipeService>()
                .AddSingleton<ICategoryService, CategoryService>()
                .AddSingleton<IDishTypeService, DishTypeService>()
                .AddSingleton<IProductService, ProductService>()
                .AddAutoMapper(typeof(RecipeMapping),
                                typeof(UserMapping),
                                typeof(CategoryMapping),
                                typeof(DishTypeMapping),
                                typeof(ProductMapping))
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();

            await SeedAsync(repo);
        }

        [Test]
        public async Task SuccessCreationOfRecipe()
        {
            var viewModel = new CreateRecipeServiceModel()
            {
                AuthorId = "TestId",
                Category = "TestCategory",
                DishType = "TestDishType",
                CookingTime = "10 min",
                Description = "TestDescription",
                Products = "10gr testProduct, 10gr testProduct1",
                Title = "TestTitle",
                ImageUrl = "testURl",
            };

            var service = serviceProvider.GetService<IRecipeService>();

            var error = await service.CreateRecipe(viewModel);

            Assert.That(error.Messages == null);
        }

        [Test]
        public async Task CreationOfRecipeMustHaveErrorMessageForAuthor()
        {
            var viewModel = new CreateRecipeServiceModel()
            {
                AuthorId = "asdasdwqd",
                Category = "TestCategory",
                DishType = "TestDishType",
                CookingTime = "10 min",
                Description = "TestDescription",
                Products = "10gr testProduct, 10gr testProduct1",
                Title = "TestTitle",
                ImageUrl = "testURl",
            };

            var service = serviceProvider.GetService<IRecipeService>();

            var error = await service.CreateRecipe(viewModel);

            Assert.That(error.Messages == UserConstants.InvalidAuthor);
        }

        [Test]
        public async Task CreationOfRecipeMustHaveErrorMessageForCategory()
        {
            var viewModel = new CreateRecipeServiceModel()
            {
                AuthorId = "TestId",
                Category = "asdasdasda",
                DishType = "TestDishType",
                CookingTime = "10 min",
                Description = "TestDescription",
                Products = "10gr testProduct, 10gr testProduct1",
                Title = "TestTitle",
                ImageUrl = "testURl",
            };

            var service = serviceProvider.GetService<IRecipeService>();

            var error = await service.CreateRecipe(viewModel);

            Assert.That(error.Messages == RecipeConstants.CategoryDoesNotExist);
        }


        [Test]
        public async Task SucceedSortRecipesWithVariables()
        {
            string dishType = "TestDishType";
            string category = "TestCategory";
            bool orderByDateAsc = false;

            var service = serviceProvider.GetService<IRecipeService>();

            var recipes = await service.GetSortRecipesForPageing(1, 9, dishType, category, orderByDateAsc);

            Assert.That(recipes.Items.Count() > 0 && recipes.TotalCount > 0);
        }

        [Test]
        public async Task SucceedSortRecipesWithVariablesWithInvalidParameters()
        {
            string dishType = "cvbddf";
            string category = "er sdf sdf";
            bool orderByDateAsc = false;

            var service = serviceProvider.GetService<IRecipeService>();

            var recipes = await service.GetSortRecipesForPageing(1, 9, dishType, category, orderByDateAsc);

            Assert.That(recipes.Items.Count() == 0 && recipes.TotalCount == 0);
        }

        [Test]
        public async Task SucceedGetRecipeById()
        {
            var service = serviceProvider.GetService<IRecipeService>();

            var id = Guid.Parse("d86dac3a-e8ca-4205-b99d-fa0d44cfbd75");

            var recipe = await service.GetById(id);

            Assert.That(recipe != null);
        }

        [Test]
        public async Task FiledGetRecipeById()
        {
            var service = serviceProvider.GetService<IRecipeService>();

            var fakeGuid = new Guid();

            var recipe = await service.GetById(fakeGuid);

            Assert.That(recipe == null);
        }

        [Test]
        public async Task SucceedTakeRecipeComment()
        {
            var service = serviceProvider.GetService<IRecipeService>();

            var guid = Guid.Parse("d86dac3a-e8ca-4205-b99d-fa0d44cfbd75");

            var comments = await service.GetRecipeCommentsPerPage(guid, 1, 5);

            var listComments = comments.ToList();

            Assert.That(listComments.Count > 0);
        }

        [Test]
        public async Task TakeRecipeCommentWithInvalidRecipeId()
        {
            var service = serviceProvider.GetService<IRecipeService>();

            var guid = Guid.Parse("d86dac3a-e8ca-4205-b99d-fa0d44cfbd71");

            var comments = await service.GetRecipeCommentsPerPage(guid, 1, 5);

            var listComments = comments.ToList();

            Assert.That(listComments.Count == 0);
        }

        [Test]
        public async Task TakeRecipeForPostWithValidId()
        {
            var service = serviceProvider.GetService<IRecipeService>();

            var guid = Guid.Parse("d86dac3a-e8ca-4205-b99d-fa0d44cfbd75");

            var recipe = await service.GetRecipeForPost(guid);

            Assert.That(recipe != null);
        }

        [Test]
        public async Task TakeRecipeForPostWithInvalidId()
        {
            var service = serviceProvider.GetService<IRecipeService>();

            var guid = Guid.Parse("d86dac3a-e8ca-4205-b99d-fa0d44cfbd71");

            var recipe = await service.GetRecipeForPost(guid);

            Assert.That(recipe == null);
        }

        [Test]
        public async Task SetRatingToRecipeWithValidModel()
        {
            var guid = Guid.Parse("d86dac3a-e8ca-4205-b99d-fa0d44cfbd75");

            var model = new RatingSetServiceModel()
            {
                Id = guid,
                RatingThreeCheck = true,
            };

            var service = serviceProvider.GetService<IRecipeService>();

            var recipe = await service.IsRatingSet(model);

            Assert.That(recipe == true);
        }

        [Test]
        public async Task SetRatingToRecipeWithInvalidModel()
        {
            var guid = Guid.Parse("d86dac3a-e8ca-4205-b99d-fa0d44cfbd71");

            var model = new RatingSetServiceModel()
            {
                Id = guid,
            };

            var service = serviceProvider.GetService<IRecipeService>();

            var recipe = await service.IsRatingSet(model);

            Assert.That(recipe == false);
        }


        [Test]
        public async Task TestPageingForRecipeComments()
        {
            var service = serviceProvider.GetService<IRecipeService>();

            var guid = Guid.Parse("d86dac3a-e8ca-4205-b99d-fa0d44cfbd75");

            var comments = await service.GetRecipeCommentsPerPage(guid, 1, 5);

            var listComments = comments.ToList();

            Assert.That(listComments.Count == 5);
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
                Type = "test2",
                ImageUrl = "TestUrl2",
                Description = "TestDesc2",
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

            var recipe3 = new Recipe()
            {
                Id = new Guid("386dac3a-e8ca-4205-b99d-fa0d44cfbd75"),
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

            var recipe4 = new Recipe()
            {
                Id = new Guid("486dac3a-e8ca-4205-b99d-fa0d44cfbd75"),
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

            var recipe5 = new Recipe()
            {
                Id = new Guid("586dac3a-e8ca-4205-b99d-fa0d44cfbd75"),
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

            var recipe6 = new Recipe()
            {
                Id = new Guid("686dac3a-e8ca-4205-b99d-fa0d44cfbd75"),
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

                new RecipeProduct
                {
                    Recipe = recipe,
                    Product = product2,
                    Quantity = "30"
                }
            };

            recipe.RecipeProducts = recipeProducts;

            var recipeComment = new RecipeComment()
            {
                Author = user,
                Content = "testContent",
                PublishedOn = DateTime.UtcNow,
                Recipe = recipe2,
                RecipeId = recipe2.Id,
                IsDeleted = false
            };

            var recipeComment2 = new RecipeComment()
            {
                Author = user,
                Content = "testContent",
                PublishedOn = DateTime.UtcNow,
                Recipe = recipe2,
                RecipeId = recipe2.Id,
                IsDeleted = false
            };

            var recipeComment3 = new RecipeComment()
            {
                Author = user,
                Content = "testContent",
                PublishedOn = DateTime.UtcNow,
                Recipe = recipe2,
                RecipeId = recipe2.Id,
                IsDeleted = false
            };

            var recipeComment4 = new RecipeComment()
            {
                Author = user,
                Content = "testContent",
                PublishedOn = DateTime.UtcNow,
                Recipe = recipe2,
                RecipeId = recipe2.Id,
                IsDeleted = false
            };

            var recipeComment5 = new RecipeComment()
            {
                Author = user,
                Content = "testContent",
                PublishedOn = DateTime.UtcNow,
                Recipe = recipe2,
                RecipeId = recipe2.Id,
                IsDeleted = false
            };

            var recipeComment6 = new RecipeComment()
            {
                Author = user,
                Content = "testContent",
                PublishedOn = DateTime.UtcNow,
                Recipe = recipe2,
                RecipeId = recipe2.Id,
                IsDeleted = false
            };

            recipe2.Comments.Add(recipeComment);
            recipe2.Comments.Add(recipeComment2);
            recipe2.Comments.Add(recipeComment3);
            recipe2.Comments.Add(recipeComment4);
            recipe2.Comments.Add(recipeComment5);
            recipe2.Comments.Add(recipeComment6);


            await repo.AddAsync(user);
            await repo.AddAsync(dishType);
            await repo.AddAsync(category);
            await repo.AddAsync(product1);
            await repo.AddAsync(product2);
            await repo.AddAsync(recipe);
            await repo.AddAsync(recipe2);
            await repo.AddAsync(recipe3);
            await repo.AddAsync(recipe4);
            await repo.AddAsync(recipe5);
            await repo.AddAsync(recipe6);
            await repo.AddAsync(recipeComment);
            await repo.AddAsync(recipeComment2);
            await repo.AddAsync(recipeComment3);
            await repo.AddAsync(recipeComment4);
            await repo.AddAsync(recipeComment5);
            await repo.AddAsync(recipeComment6);

            await repo.SaveChangesAsync();
        }
    }
}
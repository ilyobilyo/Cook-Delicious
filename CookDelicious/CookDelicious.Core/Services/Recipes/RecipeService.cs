﻿using AutoMapper;
using CookDelicious.Core.Contracts.Common.Categories;
using CookDelicious.Core.Contracts.Common.DishTypes;
using CookDelicious.Core.Contracts.Product;
using CookDelicious.Core.Contracts.Recipes;
using CookDelicious.Core.Contracts.User;
using CookDelicious.Core.Models.Comments;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Core.Models.Recipe;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Infrasturcture.Models.Identity;
using CookDelicious.Infrasturcture.Models.Recipes;
using CookDelicious.Infrasturcture.Repositories;
using CookDelicious.Models;
using Microsoft.EntityFrameworkCore;

namespace CookDelicious.Core.Services.Recipes
{
    public class RecipeService : IRecipeService
    {
        private readonly IApplicationDbRepository repo;
        private readonly ICategoryService categoryService;
        private readonly IDishTypeService dishTypeService;
        private readonly IProductService productService;
        private readonly IMapper mapper;
        private readonly IUserService userService;

        public RecipeService(IApplicationDbRepository repo,
            ICategoryService categoryService,
            IDishTypeService dishTypeService,
            IProductService productService,
            IUserService userService,
            IMapper mapper)
        {
            this.repo = repo;
            this.categoryService = categoryService;
            this.dishTypeService = dishTypeService;
            this.productService = productService;
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task<ErrorViewModel> CreateRecipe(CreateRecipeServiceModel model)
        {
            ApplicationUser author;
            Category category;
            var error = new ErrorViewModel();

            author = await repo.GetByIdAsync<ApplicationUser>(model.AuthorId);

            if (author == null)
            {
                error.Messages = "Потребителят не е валиден. Опитайте пак!";
                return error;
            }

            category = await categoryService.GetCategoryByName(model.Category);

            if (category == null)
            {
                error.Messages = "Категорията не съществува!";
                return error;
            }

            var dishType = await dishTypeService.GetDishTypeByName(model.DishType);

            var recipe = new Recipe()
            {
                Title = model.Title,
                AuthorId = model.AuthorId,
                Author = author,
                Catrgory = category,
                CategoryId = category.Id,
                CookingTime = model.CookingTime,
                Description = model.Description,
                DishType = dishType,
                DishTypeId = dishType.Id,
                ImageUrl = model.ImageUrl,
                PublishedOn = DateTime.Now,
            };

            recipe.RecipeProducts = await productService.GetProductsForCreatingRecipe(model.Products, recipe.Id);

            var categories = await categoryService.GetAllCategoryNames();

            model.Categories = categories;

            try
            {
                await repo.AddAsync(recipe);
                await repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                error.Messages = "Unexpected error. You cant add this recipe!";
            }

            return error;
        }

        public async Task<(IEnumerable<RecipeServiceModel>, int)> GetAllRecipesForPageing(int pageNumber, int pageSize)
        {
            var totalCount = await repo.All<Recipe>().CountAsync();

            var items = await repo.All<Recipe>()
                .Include(x => x.Ratings)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var itemsAsServiceModel = mapper.Map<IEnumerable<RecipeServiceModel>>(items);
            
            return (itemsAsServiceModel, totalCount);
        }

        //TODO MAPPING
        public async Task<Recipe> GetById(Guid id)
        {
            return await repo.GetByIdAsync<Recipe>(id);
        }

        public async Task<IEnumerable<RecipeCommentServiceModel>> GetRecipeCommentsPerPage(Guid Id ,int commentPage, int pageSize)
        {
            var items = await repo.All<RecipeComment>()
                .Where(x => x.RecipeId == Id && x.IsDeleted == false)
                .Skip((commentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var itemsAsServiceModel = mapper.Map<IEnumerable<RecipeCommentServiceModel>>(items);

            return itemsAsServiceModel;
        }

        //TODO MAPPING
        public async Task<RecipeServiceModel> GetRecipeForPost(Guid id)
        {
            var recipe = await repo.All<Recipe>()
                .Include(x => x.RecipeProducts)
                .ThenInclude(x => x.Product)
                .Include(x => x.Ratings)
                .Include(x => x.Comments.Where(c => c.IsDeleted == false))
                .Include(x => x.Author)
                .Include(x => x.Catrgory)
                .Include(x => x.DishType)
               .Where(x => x.Id == id)
               .FirstOrDefaultAsync();

            var recipeServiceModel = mapper.Map<RecipeServiceModel>(recipe);

            recipeServiceModel.Category = mapper.Map<CategoryServiceModel>(recipe.Catrgory);

            return recipeServiceModel;
        }

      

        public async Task<RecipeServiceModel> GetRecipeForSetRating(Guid id)
        {
            var recipe = await repo.All<Recipe>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return mapper.Map<RecipeServiceModel>(recipe);
        }

        public async Task<bool> IsRatingSet(RatingSetServiceModel model)
        {
            var recipe = await repo.GetByIdAsync<Recipe>(model.Id);

            var ratingDigit = GetRatingDigit(model);

            if (ratingDigit == 0)
            {
                return false;
            }

            recipe.Ratings.Add(new Rating()
            {
                RecipeId = model.Id,
                RatingDigit = ratingDigit
            });

            try
            {
                 await repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private int GetRatingDigit(RatingSetServiceModel model)
        {
            var digit = 0;

            if (model.RatingOneCheck)
            {
                digit = 1;
            }
            else if (model.RatingTwoCheck)
            {
                digit = 2;
            }
            else if (model.RatingThreeCheck)
            {
                digit = 3;
            }
            else if (model.RatingFourCheck)
            {
                digit = 4;
            }
            else if (model.RatingFiveCheck)
            {
                digit = 5;
            }

            return digit;
        }

    }
}

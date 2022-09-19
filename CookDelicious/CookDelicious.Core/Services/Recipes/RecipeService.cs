using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Common.Categories;
using CookDelicious.Core.Contracts.Common.DishTypes;
using CookDelicious.Core.Contracts.Product;
using CookDelicious.Core.Contracts.Recipes;
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

        public RecipeService(IApplicationDbRepository repo,
            ICategoryService categoryService,
            IDishTypeService dishTypeService,
            IProductService productService,
            IMapper mapper)
        {
            this.repo = repo;
            this.categoryService = categoryService;
            this.dishTypeService = dishTypeService;
            this.productService = productService;
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
                error.Messages = UserConstants.InvalidAuthor;
                return error;
            }

            category = await categoryService.GetCategoryByName(model.Category);

            if (category == null)
            {
                error.Messages = RecipeConstants.CategoryDoesNotExist;
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

            recipe.RecipeProducts = await productService.SetProductsForCreatingRecipe(model.Products, recipe.Id);

            var categories = await categoryService.GetAllCategoryNames();

            model.Categories = categories;

            try
            {
                await repo.AddAsync(recipe);
                await repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                error.Messages = RecipeConstants.UnexpectedErrorRecipe;
            }

            return error;
        }

        public async Task<(IEnumerable<RecipeServiceModel>, int)> GetSortRecipesForPageing(int pageNumber, int pageSize, SortServiceModel model)
        {
            (var sortRecipes, var totalCount) = await GetSortRecipes(pageNumber, pageSize, model.DishType, model.Category, model.Date);

            var itemsAsServiceModel = mapper.Map<IEnumerable<RecipeServiceModel>>(sortRecipes);

            return (itemsAsServiceModel, totalCount);
        }

        public async Task<(IEnumerable<RecipeServiceModel>, int)> GetSortRecipesForPageing(int pageNumber,
            int pageSize,
            string dishType,
            string category,
            bool orderByDateAsc)
        {
            (var sortRecipes, var totalCount) = await GetSortRecipes(pageNumber, pageSize, dishType, category, orderByDateAsc);

            var itemsAsServiceModel = mapper.Map<IEnumerable<RecipeServiceModel>>(sortRecipes);

            return (itemsAsServiceModel, totalCount);
        }


        public async Task<RecipeServiceModel> GetById(Guid id)
        {
            var recipe = await repo.GetByIdAsync<Recipe>(id);

            return mapper.Map<RecipeServiceModel>(recipe);
        }

        public async Task<IEnumerable<RecipeCommentServiceModel>> GetRecipeCommentsPerPage(Guid Id, int commentPage, int pageSize)
        {
            var items = await repo.All<RecipeComment>()
                .Include(x => x.Author)
                .Where(x => x.RecipeId == Id && x.IsDeleted == false)
                .Skip((commentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var itemsAsServiceModel = mapper.Map<IEnumerable<RecipeCommentServiceModel>>(items);

            return itemsAsServiceModel;
        }

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

            if (recipe == null)
            {
                return null;
            }

            var recipeServiceModel = mapper.Map<RecipeServiceModel>(recipe);

            recipeServiceModel.Category = mapper.Map<CategoryServiceModel>(recipe.Catrgory);

            return recipeServiceModel;
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



        private async Task<(IEnumerable<Recipe>, int)> GetSortRecipes(int pageNumber,
            int pageSize,
            string dishType,
            string category,
            bool orderByDateAsc)
        {
            var totalCount = 0;

            var items = new List<Recipe>();

            if (dishType == RecipeConstants.DefaultSortDishType && category == RecipeConstants.DefaultSortCategory)
            {
                totalCount = await repo.All<Recipe>()
               .CountAsync();

                if (orderByDateAsc)
                {
                    items = await repo.All<Recipe>()
                   .Include(x => x.DishType)
                   .Include(x => x.Catrgory)
                   .Include(x => x.Ratings)
                   .Where(x => x.IsDeleted == false)
                   .OrderBy(x => x.PublishedOn.Date)
                   .Skip((pageNumber - 1) * pageSize)
                   .Take(pageSize)
                   .ToListAsync();
                }
                else
                {
                    items = await repo.All<Recipe>()
                   .Include(x => x.DishType)
                   .Include(x => x.Catrgory)
                   .Include(x => x.Ratings)
                   .Where(x => x.IsDeleted == false)
                   .OrderByDescending(x => x.PublishedOn.Date)
                   .Skip((pageNumber - 1) * pageSize)
                   .Take(pageSize)
                   .ToListAsync();
                }
            }
            else
            {
                totalCount = await GetSortedRecipesTotalCount(dishType, category);

                if (totalCount == 0)
                {
                    totalCount = await GetRecipeTotalCountWithOneOfTheSortParameters(dishType, category);

                    if (orderByDateAsc)
                    {
                        items = await repo.All<Recipe>()
                        .Include(x => x.DishType)
                        .Include(x => x.Catrgory)
                        .Include(x => x.Ratings)
                        .Where(x => (x.DishType.Name == dishType
                        || x.Catrgory.Name == category) && x.IsDeleted == false)
                         .OrderBy(x => x.PublishedOn.Date)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();
                    }
                    else
                    {
                        items = await repo.All<Recipe>()
                        .Include(x => x.DishType)
                        .Include(x => x.Catrgory)
                        .Include(x => x.Ratings)
                        .Where(x => (x.DishType.Name == dishType
                        || x.Catrgory.Name == category) && x.IsDeleted == false)
                         .OrderByDescending(x => x.PublishedOn.Date)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();
                    }

                }
                else
                {
                    if (orderByDateAsc)
                    {
                        items = await repo.All<Recipe>()
                        .Include(x => x.DishType)
                        .Include(x => x.Catrgory)
                        .Include(x => x.Ratings)
                        .Where(x => (x.DishType.Name == dishType
                        && x.Catrgory.Name == category) && x.IsDeleted == false)
                         .OrderBy(x => x.PublishedOn.Date)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();
                    }
                    else
                    {
                        items = await repo.All<Recipe>()
                        .Include(x => x.DishType)
                        .Include(x => x.Catrgory)
                        .Include(x => x.Ratings)
                        .Where(x => (x.DishType.Name == dishType
                        && x.Catrgory.Name == category) && x.IsDeleted == false)
                         .OrderByDescending(x => x.PublishedOn.Date)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();
                    }

                }
            }

            return (items, totalCount);
        }

        private async Task<int> GetRecipeTotalCountWithOneOfTheSortParameters(string dishType, string category)
        {
            return await repo.All<Recipe>()
                .Where(x => (x.DishType.Name == dishType
                || x.Catrgory.Name == category) && x.IsDeleted == false)
                .CountAsync();
        }

        private async Task<int> GetSortedRecipesTotalCount(string dishType, string category)
        {
            return await repo.All<Recipe>()
           .Where(x => x.DishType.Name == dishType
           && x.Catrgory.Name == category && x.IsDeleted == false)
           .CountAsync();
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

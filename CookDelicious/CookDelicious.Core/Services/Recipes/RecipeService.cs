using AutoMapper;
using CookDelicious.Core.Contracts.Common.Categories;
using CookDelicious.Core.Contracts.Common.DishTypes;
using CookDelicious.Core.Contracts.Product;
using CookDelicious.Core.Contracts.Recipes;
using CookDelicious.Core.Contracts.User;
using CookDelicious.Core.Models.Comments;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Core.Models.Recipe;
using CookDelicious.Core.Service.Models;
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

        //TODO MAPPING
        public async Task<ErrorViewModel> CreateRecipe(CreateRecipeViewModel model)
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

            //category = await categoryService.GetCategoryByName(model.Category);

            //if (category == null)
            //{
            //    error.Messages = "Категорията не съществува!";
            //    return error;
            //}

            //var dishType = await dishTypeService.GetDishTypeByName(model.DishType);

            var recipe = new Recipe()
            {
                Title = model.Title,
                AuthorId = model.AuthorId,
                Author = author,
                //Catrgory = category,
                //CategoryId = category.Id,
                CookingTime = model.CookingTime,
                Description = model.Description,
                //DishType = dishType,
                //DishTypeId = dishType.Id,
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

        //TODO MAPPING
        public async Task<IEnumerable<AllRecipeViewModel>> GetAllRecipes(int pageNumber)
        {
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            int pageSize = 12;


            return await PagingList<AllRecipeViewModel>.CreateAsync(repo.All<Recipe>()
                .Select(p => new AllRecipeViewModel
                {
                    Id = p.Id.ToString(),
                    ImageUrl = p.ImageUrl,
                    //Ratings = p.Ratings,
                    Title = p.Title,
                }),
                pageNumber,
                pageSize);
        }

        //TODO MAPPING
        public async Task<Recipe> GetById(Guid id)
        {
            return await repo.GetByIdAsync<Recipe>(id);
        }

        //TODO MAPPING
        public async Task<RecipePostViewModel> GetRecipeForPost(Guid id, int commentPage)
        {
            if (commentPage == 0)
            {
                commentPage = 1;
            }

            int pageSize = 5;

            var products = await productService.GetProductsForRecipePost(id);

            var recipe = await repo.All<Recipe>()
               .Where(x => x.Id == id)
               .Select(x => new RecipePostViewModel()
               {
                   Id = id,
                   ImageUrl = x.ImageUrl,
                   Author = x.Author,
                   Category = x.Catrgory.Name,
                   CookingTime = x.CookingTime,
                   Description = x.Description,
                   //Ratings = x.Ratings,
                   Title = x.Title,
                   PublishedOn = x.PublishedOn.ToString("dd/MM/yyyy"),
                   DishType = x.DishType.Name,
                   Products = products
               })
               .FirstOrDefaultAsync();

            recipe.Comments = await PagingList<CommentViewModel>.CreateAsync(repo.All<RecipeComment>()
                .Where(x => x.IsDeleted == false && x.RecipeId == recipe.Id)
                .Select(x => new CommentViewModel
                {
                    Id = x.Id,
                    AuthorName = x.Author.UserName,
                    Content = x.Content,
                }),
                commentPage,
                pageSize);

            return recipe;
        }

        //TODO MAPPING
        public async Task<RatingViewModel> GetRecipeForSetRating(Guid id)
        {
            return await repo.All<Recipe>()
                .Where(x => x.Id == id)
                .Select(x => new RatingViewModel()
                {
                    Id = id,
                    ImageUrl = x.ImageUrl,
                    Title = x.Title,
                    DatePublishedOn = x.PublishedOn.ToString("dd"),
                    MonthPublishedOn = x.PublishedOn.ToString("MMMM"),
                    YearPublishedOn = x.PublishedOn.ToString("yyyy"),
                }).FirstOrDefaultAsync();

        }

        //TODO MAPPING
        public async Task<bool> IsRatingSet(RatingViewModel model)
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

        private int GetRatingDigit(RatingViewModel model)
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

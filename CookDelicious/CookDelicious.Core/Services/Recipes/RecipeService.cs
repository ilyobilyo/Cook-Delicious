using CookDelicious.Core.Contracts.Common.Categories;
using CookDelicious.Core.Contracts.Common.DishTypes;
using CookDelicious.Core.Contracts.Product;
using CookDelicious.Core.Contracts.Recipe;
using CookDelicious.Core.Contracts.User;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Core.Models.Recipe;
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
        private readonly IUserService userService;

        public RecipeService(IApplicationDbRepository repo,
            ICategoryService categoryService,
            IDishTypeService dishTypeService,
            IProductService productService,
            IUserService userService)
        {
            this.repo = repo;
            this.categoryService = categoryService;
            this.dishTypeService = dishTypeService;
            this.productService = productService;
            this.userService = userService;
        }

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
                    Rating = p.Rating,
                    Title = p.Title,
                }),
                pageNumber,
                pageSize);
        }

        public async Task<RecipePostViewModel> GetRecipeForPost(Guid id)
        {
            var products = await productService.GetProductsForRecipePost(id);

            return await repo.All<Recipe>()
                .Where(x => x.Id == id)
                .Select(x => new RecipePostViewModel()
                {
                    Id = id,
                    ImageUrl = x.ImageUrl,
                    Author = x.Author,
                    Category = x.Catrgory.Name,
                    CookingTime = x.CookingTime,
                    Description = x.Description,
                    Title = x.Title,
                    PublishedOn = x.PublishedOn.ToString("dd/MM/yyyy"),
                    DishType = x.DishType.Name,
                    Products = products
                })
                .FirstOrDefaultAsync();
        }
    }
}

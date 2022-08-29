using CookDelicious.Core.Contracts.Common.Categories;
using CookDelicious.Core.Contracts.Common.DishTypes;
using CookDelicious.Core.Contracts.Product;
using CookDelicious.Core.Contracts.Recipe;
using CookDelicious.Core.Contracts.User;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Core.Models.Recipe;
using CookDelicious.Infrasturcture.Models.Identity;
using CookDelicious.Infrasturcture.Models.Recipes;
using CookDelicious.Infrasturcture.Repositories;
using CookDelicious.Models;

namespace CookDelicious.Core.Services.Recipes
{
    public class RecipeService : IRecipeService
    {
        private readonly IApplicationDbRepository repo;
        private readonly ICategoryService categoryService;
        private readonly IDishTypeService dishTypeService;
        private readonly IProductService productService;

        public RecipeService(IApplicationDbRepository repo,
            ICategoryService categoryService,
            IDishTypeService dishTypeService,
            IProductService productService)
        {
            this.repo = repo;
            this.categoryService = categoryService;
            this.dishTypeService = dishTypeService;
            this.productService = productService;
        }

        public async Task<IEnumerable<ErrorViewModel>> CreateRecipe(CreateRecipeViewModel model)
        {
            List<ErrorViewModel> errors = new List<ErrorViewModel>();


            var author = await repo.GetByIdAsync<ApplicationUser>(model.AuthorId);

            if (author == null)
            {
                errors.Add(new ErrorViewModel() { Messages = "Потребителят не е валиден. Опитайте пак!" });
                return errors;
            }

            var category = await categoryService.GetCategoryByName(model.Category);

            if (category == null)
            {
                errors.Add(new ErrorViewModel() { Messages = "Категорията не съществува!" });
                return errors;
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

            try
            {
                await repo.AddAsync(recipe);
                await repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                errors.Add(new ErrorViewModel() { Messages = "Unexpected error. You cant add this recipe!" });
            }

            return errors;
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
    }
}

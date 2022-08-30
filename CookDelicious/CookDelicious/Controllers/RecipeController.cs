using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Common.Categories;
using CookDelicious.Core.Contracts.Recipe;
using CookDelicious.Core.Models.Recipe;
using CookDelicious.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Controllers
{
    [Authorize(Roles = "Administrator, User")]
    public class RecipeController : BaseController
    {
        private readonly IRecipeService recipeService;
        private readonly ICategoryService categoryService;

        public RecipeController(IRecipeService recipeService, ICategoryService categoryService)
        {
            this.recipeService = recipeService;
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> All(int pageNumber)
        {
            var recipes = await recipeService.GetAllRecipes(pageNumber);

            return View(recipes);
        }

        public async Task<IActionResult> CreateRecipe([FromRoute]string Id)
        {
            var categories = await categoryService.GetAllCategoryNames();

            var model = new CreateRecipeViewModel()
            {
                Categories = categories
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecipe(CreateRecipeViewModel model, [FromRoute] string Id)
        {
            if (!ModelState.IsValid)    
            {
                ViewData[MessageConstant.ErrorMessage] = "Невярно попълнена информация!";
                return View(model);
            }

            model.AuthorId = Id;

            var error = await recipeService.CreateRecipe(model);

            if (error.Messages != null)
            {
                ViewData[MessageConstant.ErrorMessage] = error.Messages;
                return View(model);
            }
            else
            {
                ViewData[MessageConstant.SuccessMessage] = "Вие създадохте рецептата успешно!";
                return View(model);
            }
        }

        public async Task<IActionResult> RecipePost([FromRoute] Guid Id)
        {
            var model = await recipeService.GetRecipeForPost(Id);

            return View(model);
        }
    }
}

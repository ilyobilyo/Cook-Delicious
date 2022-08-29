using CookDelicious.Core.Constants;
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

        public RecipeController(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
        }

        public async Task<IActionResult> All(int pageNumber)
        {
            var recipes = await recipeService.GetAllRecipes(pageNumber);

            return View(recipes);
        }

        public IActionResult CreateRecipe([FromRoute]string Id)
        {
            return View();
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

            var errors = await recipeService.CreateRecipe(model);

            if (errors.Count() > 0)
            {
                ViewData[MessageConstant.ErrorMessage] = errors.Select(x => x.Messages);
                return View(model);
            }
            else
            {
                ViewData[MessageConstant.SuccessMessage] = "Вие създадохте рецептата успешно!";
                return Redirect("/");
            }
        }
    }
}

using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Comments;
using CookDelicious.Core.Contracts.Common.Categories;
using CookDelicious.Core.Contracts.Recipes;
using CookDelicious.Core.Models.Comments;
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
        private readonly ICommentService commentService;

        public RecipeController(IRecipeService recipeService, ICategoryService categoryService, ICommentService commentService)
        {
            this.recipeService = recipeService;
            this.categoryService = categoryService;
            this.commentService = commentService;
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
        public async Task<IActionResult> RecipePost([FromRoute] Guid Id, int commentPage)
        {
            var model = await recipeService.GetRecipeForPost(Id, commentPage);

            return View(model);
        }

        public async Task<IActionResult> Rating([FromRoute] Guid Id)
        {
            var model = await recipeService.GetRecipeForSetRating(Id);

            ViewData[MessageConstant.WarningMessage] = "Изберете звезда от 1 до 5 за успешен вод !";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Rating(RatingViewModel model)
        {

            if (await recipeService.IsRatingSet(model))
            {
                ViewData[MessageConstant.SuccessMessage] = "Вашият вот е успешен!";
            }
            else
            {
                ViewData[MessageConstant.ErrorMessage] = "Вашият вот не е успешен! Опитайте пак.";
            }

            model = await recipeService.GetRecipeForSetRating(model.Id);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> PostComment([FromRoute] Guid Id, CommentViewModel model)
        {
            var comment = await commentService.PostCommentForRecipe(Id, model);

            return RedirectToAction(nameof(RecipePost), new { Id = Id });
        }

        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> DeleteRecipeComment([FromRoute] Guid Id, [FromQuery] Guid recipeId)
        {
            var IsDeleted = await commentService.DeleteRecipeComment(Id);

            if (!IsDeleted)
            {
                return BadRequest("Неуспешно изтриване!");
            }

            return RedirectToAction(nameof(RecipePost), new { Id = recipeId });
        }
    }
}

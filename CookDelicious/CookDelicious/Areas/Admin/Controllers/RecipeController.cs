using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Models.Admin.Comments;
using CookDelicious.Core.Models.Admin.Recipe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Areas.Admin.Controllers
{
    public class RecipeController : BaseController
    {
        private readonly IRecipeServiceAdmin recipeService;
        private readonly ICommentServiceAdmin commentService;
        private readonly IMapper mapper;

        public RecipeController(IRecipeServiceAdmin recipeService, ICommentServiceAdmin commentService, IMapper mapper)
        {
            this.recipeService = recipeService;
            this.commentService = commentService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> DeleteRecipeComment([FromRoute] Guid id)
        {
            await commentService.DeleteRecipeComment(id);

            return Redirect("/Admin/User/ManageUsers");
        }

        [Authorize(Roles = UserConstants.Roles.Administrator)]
        public async Task<IActionResult> RecipeComments([FromRoute] string id)
        {
            var commentsServiceModels = await commentService.GetRecipeComments(id);

            var commentsViewModel = mapper.Map<List<AdminCommentViewModel>>(commentsServiceModels);

            return View(commentsViewModel);
        }

        public async Task<IActionResult> DeleteRecipe([FromRoute] Guid id)
        {
            await recipeService.DeleteRecipe(id);

            return Redirect("/Admin/User/ManageUsers");
        }

        public async Task<IActionResult> UserRecipes([FromRoute] string id)
        {
            var userRecipesServiceModel = await recipeService.GetUserRecipes(id);

            var userRecipesViewModel = mapper.Map<List<ManageRecipeViewModel>>(userRecipesServiceModel);

            return View(userRecipesViewModel);
        }
    }
}

using CookDelicious.Core.Contracts.Admin;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Areas.Admin.Controllers
{
    public class RecipeController : BaseController
    {
        private readonly ICommentServiceAdmin commentService;

        public RecipeController(ICommentServiceAdmin commentService)
        {
            this.commentService = commentService;
        }

        public async Task<IActionResult> DeleteRecipeComment([FromRoute] Guid id)
        {
            await commentService.DeleteRecipeComment(id);

            return Redirect("/Admin/User/ManageUsers");
        }
    }
}

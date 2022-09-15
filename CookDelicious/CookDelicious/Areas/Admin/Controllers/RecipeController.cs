using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Models.Admin.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Areas.Admin.Controllers
{
    public class RecipeController : BaseController
    {
        private readonly ICommentServiceAdmin commentService;
        private readonly IMapper mapper;

        public RecipeController(ICommentServiceAdmin commentService, IMapper mapper)
        {
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
    }
}

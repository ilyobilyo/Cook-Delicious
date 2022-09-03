using CookDelicious.Core.Contracts.Admin;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Areas.Admin.Controllers
{
    public class CommentController : BaseController
    {
        private readonly ICommentServiceAdmin commentServiceAdmin;

        public CommentController(ICommentServiceAdmin commentServiceAdmin)
        {
            this.commentServiceAdmin = commentServiceAdmin;
        }

        public async Task<IActionResult> AllCommets(int pageNumber)
        {
            var comments = await commentServiceAdmin.GetAllComments(pageNumber);

            return View();
        }
    }
}

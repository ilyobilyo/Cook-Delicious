using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Models.Admin.Forum;
using CookDelicious.Core.Service.Models.InputServiceModels;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Areas.Admin.Controllers
{
    public class ForumController : BaseController
    {
        private readonly IForumServiceAdmin forumService;
        private readonly ICommentServiceAdmin commentService;
        private readonly IMapper mapper;

        public ForumController(IForumServiceAdmin forumService,
            ICommentServiceAdmin commentService,
            IMapper mapper)
        {
            this.forumService = forumService;
            this.commentService = commentService;
            this.mapper = mapper;
        }

        public IActionResult CreatePostCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePostCategory(CreatePostCategoryViewModel model)
        {
            var inputModel = mapper.Map<CreatePostCategoryInputModel>(model);

            var error = await forumService.CreatePostCategory(inputModel);

            if (error != null)
            {
                ViewData[MessageConstant.ErrorMessage] = error.Messages;
                return View(model);
            }
            else
            {
                ViewData[MessageConstant.SuccessMessage] = "Успешен запис";
            }

            return View();
        }

        public async Task<IActionResult> DeleteForumComment([FromRoute] Guid id)
        {
            await commentService.DeleteForumComment(id);

            return Redirect("/Admin/User/ManageUsers");
        }
    }
}

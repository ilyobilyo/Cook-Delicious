using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Comments;
using CookDelicious.Core.Contracts.Forum;
using CookDelicious.Core.Contracts.Pageing;
using CookDelicious.Core.Contracts.User;
using CookDelicious.Core.Models.Comments;
using CookDelicious.Core.Models.Forum;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Core.Models.Sorting;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Controllers
{
    public class ForumController : BaseController
    {
        private readonly IForumService forumService;
        private readonly ICommentService commentService;
        private readonly IMapper mapper;
        private readonly IPageingService pageingService;

        public ForumController(IForumService forumService, 
            ICommentService commentService,
            IMapper mapper,
            IPageingService pageingService)
        {
            this.forumService = forumService;
            this.commentService = commentService;
            this.mapper = mapper;
            this.pageingService = pageingService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Home(int pageNumber, string sortCategory = null)
        {
            var model = await pageingService.GetForumHomePagedModel(pageNumber, sortCategory);

            return View(model);
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.User}")]
        public async Task<IActionResult> CreatePost()
        {
            var categories = await forumService.GetAllPostCategoryNames();

            var model = new CreatePostViewModel()
            {
                Categories = categories,
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.User}")]
        public async Task<IActionResult> CreatePost(CreatePostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("_Error", PostsConstants.InvalidPostData);
            }

            var inputModel = mapper.Map<CreateForumPostInputModel>(model);

            var error = await forumService.CreatePost(inputModel, User.Identity.Name);

            if (error != null)
            {
                ViewData[MessageConstant.ErrorMessage] = error.Messages;
            }
            else
            {
                ViewData[MessageConstant.SuccessMessage] = PostsConstants.PostSuccessfullyPublished;
            }

            var categories = await forumService.GetAllPostCategoryNames();

            model.Categories = categories;

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ForumPost([FromRoute]Guid Id, int commentPage)
        {
            var forumPostViewModel = await pageingService.GetForumPostPagedModel(Id, commentPage);

            return View(forumPostViewModel);
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.User}")]
        public async Task<IActionResult> DeletePost([FromRoute]Guid Id)
        {
            var IsDeleted = await forumService.DeletePost(Id);

            if (!IsDeleted)
            {
                return BadRequest(MessageConstant.DeleteFailed);
            }

            return RedirectToAction(nameof(Home));
        }

        [HttpPost]
        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.User}")]
        public async Task<IActionResult> PostComment([FromRoute] Guid Id, CommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("_Error", new ErrorViewModel() { Messages = CommentConstants.CommentContentLength });
            }

            var commentInputModel = mapper.Map<PostCommentInputModel>(model);

            var error = await commentService.PostCommentForPost(Id, commentInputModel);

            if (error != null)
            {
                return View("_Error", error);
            }

            return RedirectToAction(nameof(ForumPost), new { Id = Id });
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.User}")]
        public async Task<IActionResult> DeletePostComment([FromRoute] Guid Id, [FromQuery] Guid postId)
        {
            var IsDeleted = await commentService.DeletePostComment(Id);

            if (!IsDeleted)
            {
                return BadRequest(MessageConstant.DeleteFailed);
            }

            return RedirectToAction(nameof(ForumPost), new { Id = postId });
        }
    }
}

using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Comments;
using CookDelicious.Core.Contracts.Forum;
using CookDelicious.Core.Contracts.User;
using CookDelicious.Core.Models.Comments;
using CookDelicious.Core.Models.Forum;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Core.Service.Models.InputServiceModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Controllers
{
    public class ForumController : BaseController
    {
        private readonly IForumService forumService;
        private readonly ICommentService commentService;
        private readonly IMapper mapper;

        public ForumController(IForumService forumService, 
            ICommentService commentService,
            IMapper mapper)
        {
            this.forumService = forumService;
            this.commentService = commentService;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Home(int pageNumber)
        {
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            int pageSize = 6;

            var (postsServiceModels, totalPostsCount) = await forumService.GetAllPostsForPageing(pageNumber, pageSize);

            var postsViewModel = mapper.Map<List<AllForumPostViewModel>>(postsServiceModels);

            var postsPageingList = new PagingList<AllForumPostViewModel>(postsViewModel, totalPostsCount, pageNumber, pageSize);

            var categories = await forumService.GetAllPostCategoryNames();

            var archive = await forumService.GetArchive();

            var model = new ForumHomeViewModel()
            {
                Categories = categories,
                Posts = postsPageingList,
                Archive = archive
            };

            return View(model);
        }

        [Authorize(Roles = "Administrator, User")]
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
        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> CreatePost(CreatePostViewModel model)
        {
            var inputModel = mapper.Map<CreateForumPostInputModel>(model);

            var error = await forumService.CreatePost(inputModel, User.Identity.Name);

            if (error != null)
            {
                ViewData[MessageConstant.ErrorMessage] = error.Messages;
            }
            else
            {
                ViewData[MessageConstant.SuccessMessage] = "Вие публикувахте поста успешно!";
            }

            var categories = await forumService.GetAllPostCategoryNames();

            model.Categories = categories;

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ForumPost([FromRoute]Guid Id, int commentPage)
        {
            if (commentPage == 0)
            {
                commentPage = 1;
            }

            int pageSize = 5;

            var post = await forumService.GetPostServiceModelById(Id);

            var commentsPerPage = await forumService.GetCommentsPerPage(Id, commentPage, pageSize);

            var commentsViewModels = mapper.Map<List<CommentViewModel>>(commentsPerPage);

            var commentPageingList = new PagingList<CommentViewModel>(commentsViewModels, post.ForumComments.Count, commentPage, pageSize);

            var forumPostrViewModel = mapper.Map<ForumPostViewModel>(post);

            forumPostrViewModel.Comments = commentPageingList;

            return View(forumPostrViewModel);
        }

        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> DeletePost([FromRoute]Guid Id)
        {
            var IsDeleted = await forumService.DeletePost(Id);

            if (!IsDeleted)
            {
                return BadRequest("Неуспешно изтриване!");
            }

            return RedirectToAction(nameof(Home));
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> PostComment([FromRoute] Guid Id, CommentViewModel model)
        {
            var commentInputModel = mapper.Map<PostCommentInputModel>(model);

            var comment = await commentService.PostCommentForPost(Id, commentInputModel);

            return RedirectToAction(nameof(ForumPost), new { Id = Id });
        }

        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> DeletePostComment([FromRoute] Guid Id, [FromQuery] Guid postId)
        {
            var IsDeleted = await commentService.DeletePostComment(Id);

            if (!IsDeleted)
            {
                return BadRequest("Неуспешно изтриване!");
            }

            return RedirectToAction(nameof(ForumPost), new { Id = postId });
        }
    }
}

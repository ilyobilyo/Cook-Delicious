using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Comments;
using CookDelicious.Core.Contracts.Common.Categories;
using CookDelicious.Core.Contracts.Recipes;
using CookDelicious.Core.Models.Comments;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Core.Models.Recipe;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
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
        private readonly IMapper mapper;

        public RecipeController(IRecipeService recipeService, ICategoryService categoryService, ICommentService commentService, IMapper mapper)
        {
            this.recipeService = recipeService;
            this.categoryService = categoryService;
            this.commentService = commentService;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        public async Task<IActionResult> All(int pageNumber)
        {
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            int pageSize = 1;

            (var recipesServiceModels, var totalCount) = await recipeService.GetAllRecipesForPageing(pageNumber, pageSize);

            var recipesViewModel = mapper.Map<List<AllRecipeViewModel>>(recipesServiceModels);

            var pagingList = new PagingList<AllRecipeViewModel>(recipesViewModel, totalCount, pageNumber, pageSize);

            return View(pagingList);
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

            var createRecipeServiceModel = mapper.Map<CreateRecipeServiceModel>(model);

            var error = await recipeService.CreateRecipe(createRecipeServiceModel);

            model = mapper.Map<CreateRecipeViewModel>(createRecipeServiceModel);

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
            if (commentPage == 0)
            {
                commentPage = 1;
            }

            int pageSize = 5;

            var serviceModel = await recipeService.GetRecipeForPost(Id);

            var commentsServiceModel = await recipeService.GetRecipeCommentsPerPage(Id, commentPage, pageSize);

            var commentsViewModel = mapper.Map<List<CommentViewModel>>(commentsServiceModel);

            var commentsPagingList = new PagingList<CommentViewModel>(commentsViewModel, serviceModel.Comments.Count(), commentPage, pageSize);

            var recipePostViewModel = mapper.Map<RecipePostViewModel>(serviceModel);

            recipePostViewModel.Comments = commentsPagingList;

            return View(recipePostViewModel);
        }

        public async Task<IActionResult> Rating([FromRoute] Guid Id)
        {
            var serviceModel = await recipeService.GetRecipeForSetRating(Id);

            var viewModel = mapper.Map<RatingViewModel>(serviceModel);

            ViewData[MessageConstant.WarningMessage] = "Изберете звезда от 1 до 5 за успешен вод !";
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Rating(RatingViewModel model)
        {
            var ratingServiceModel = mapper.Map<RatingSetServiceModel>(model);

            if (await recipeService.IsRatingSet(ratingServiceModel))
            {
                ViewData[MessageConstant.SuccessMessage] = "Вашият вот е успешен!";
            }
            else
            {
                ViewData[MessageConstant.ErrorMessage] = "Вашият вот не е успешен! Опитайте пак.";
            }

            var serviceModel = await recipeService.GetRecipeForSetRating(ratingServiceModel.Id);

            var viewModel = mapper.Map<RatingViewModel>(serviceModel);

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> PostComment([FromRoute] Guid Id, CommentViewModel model)
        {
            var commentServiceModel = mapper.Map<PostCommentInputModel>(model);

            var error = await commentService.PostCommentForRecipe(Id, commentServiceModel);

            if (error != null)
            {
                return View("_Error", error);
            }

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

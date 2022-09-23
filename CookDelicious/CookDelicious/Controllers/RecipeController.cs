using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Comments;
using CookDelicious.Core.Contracts.Common.Categories;
using CookDelicious.Core.Contracts.Recipes;
using CookDelicious.Core.Models.Comments;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Core.Models.Recipe;
using CookDelicious.Core.Models.Sorting;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Controllers
{
    [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.User}")]
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

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> All(int pageNumber, RecipePagingViewModel sort)
        {
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            int pageSize = PageConstants.RecipeAllPageSize;

            var sortServiceModel = mapper.Map<SortServiceModel>(sort.Sorting);

            (var recipesServiceModels, var totalCount) = await recipeService.GetSortRecipesForPageing(pageNumber, pageSize, sortServiceModel);

            var recipesViewModel = mapper.Map<List<AllRecipeViewModel>>(recipesServiceModels);

            var pagingViewModel = new RecipePagingViewModel()
            {
                PagedList = new PagingList<AllRecipeViewModel>(recipesViewModel, totalCount, pageNumber, pageSize),
                Sorting = new SortRecipeViewModel() { DishType = sortServiceModel.DishType, Category = sortServiceModel.Category, Date = sortServiceModel.Date },
                Categories = await categoryService.GetAllCategoryNames()
            };

            return View(pagingViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All(int pageNumber,
            [FromQuery] string dishType,
            [FromQuery] string category, 
            [FromQuery] bool dateAsc)
        {
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            int pageSize = PageConstants.RecipeAllPageSize;

            (var recipesServiceModels, var totalCount) = await recipeService.GetSortRecipesForPageing(pageNumber, pageSize, dishType, category, dateAsc);

            var recipesViewModel = mapper.Map<List<AllRecipeViewModel>>(recipesServiceModels);

            var pagingViewModel = new RecipePagingViewModel() 
            { 
                PagedList = new PagingList<AllRecipeViewModel>(recipesViewModel, totalCount, pageNumber, pageSize), 
                Sorting = new SortRecipeViewModel() { DishType = dishType, Category = category, Date = dateAsc },
                Categories = await categoryService.GetAllCategoryNames()
            };

            return View(pagingViewModel);
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
                ViewData[MessageConstant.ErrorMessage] = RecipeConstants.RecipeInvalidInformation;
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
                ViewData[MessageConstant.SuccessMessage] = RecipeConstants.RecipeSuccessfullyPublished;
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

            int pageSize = PageConstants.RecipeCommentPageSize;

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
            var serviceModel = await recipeService.GetById(Id);

            var viewModel = mapper.Map<RatingViewModel>(serviceModel);

            ViewData[MessageConstant.WarningMessage] = RecipeConstants.RatingReferralMessage;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Rating(RatingViewModel model)
        {
            var ratingServiceModel = mapper.Map<RatingSetServiceModel>(model);

            if (await recipeService.IsRatingSet(ratingServiceModel))
            {
                ViewData[MessageConstant.SuccessMessage] = RecipeConstants.RatingSuccesfullyRate;
            }
            else
            {
                ViewData[MessageConstant.ErrorMessage] = RecipeConstants.RatingFailedlRate;
            }

            var serviceModel = await recipeService.GetById(ratingServiceModel.Id);

            var viewModel = mapper.Map<RatingViewModel>(serviceModel);

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.User}")]
        public async Task<IActionResult> PostComment([FromRoute] Guid Id, CommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("_Error", new ErrorViewModel() { Messages = CommentConstants.CommentContentLength });
            }

            var commentServiceModel = mapper.Map<PostCommentInputModel>(model);

            var error = await commentService.PostCommentForRecipe(Id, commentServiceModel);

            if (error != null)
            {
                return View("_Error", error);
            }

            return RedirectToAction(nameof(RecipePost), new { Id = Id });
        }

        [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.User}")]
        public async Task<IActionResult> DeleteRecipeComment([FromRoute] Guid Id, [FromQuery] Guid recipeId)
        {
            var IsDeleted = await commentService.DeleteRecipeComment(Id);

            if (!IsDeleted)
            {
                return BadRequest(MessageConstant.DeleteFailed);
            }

            return RedirectToAction(nameof(RecipePost), new { Id = recipeId });
        }
    }
}

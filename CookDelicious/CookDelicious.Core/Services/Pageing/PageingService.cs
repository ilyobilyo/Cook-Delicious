using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Blog;
using CookDelicious.Core.Contracts.Common.Categories;
using CookDelicious.Core.Contracts.Forum;
using CookDelicious.Core.Contracts.Pageing;
using CookDelicious.Core.Contracts.Product;
using CookDelicious.Core.Contracts.Recipes;
using CookDelicious.Core.Models.Blog;
using CookDelicious.Core.Models.Comments;
using CookDelicious.Core.Models.Forum;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Core.Models.Product;
using CookDelicious.Core.Models.Recipe;
using CookDelicious.Core.Models.Sorting;
using CookDelicious.Core.Service.Models;

namespace CookDelicious.Core.Services.Pageing
{
    public class PageingService : IPageingService
    {
        private readonly IMapper mapper;
        private readonly IBlogService blogService;
        private readonly IForumService forumService;
        private readonly IProductService productService;
        private readonly IRecipeService recipeService;
        private readonly ICategoryService categoryService;

        public PageingService(IBlogService blogService,
            IMapper mapper,
            IForumService forumService,
            IProductService productService,
             IRecipeService recipeService,
             ICategoryService categoryService)
        {
            this.mapper = mapper;
            this.blogService = blogService;
            this.forumService = forumService;
            this.productService = productService;
            this.recipeService = recipeService;
            this.categoryService = categoryService;
        }

        public async Task<BlogHomeViewModel> GetBlogHomePagedModel(int pageNumber, string blogPostCategory = null, int? sortMonth = null)
        {
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            int pageSize = PageConstants.BlogHomePageSize;

            var (blogPostsServiceModels, totalBlogPostsCount) = await blogService.GetAllSortBlogPostsForPageing(pageNumber, pageSize, blogPostCategory, sortMonth);

            var blogPostsViewModel = mapper.Map<List<BlogPostViewModel>>(blogPostsServiceModels);

            var blogPostsPageingList = new PagingList<BlogPostViewModel>(blogPostsViewModel, totalBlogPostsCount, pageNumber, pageSize);

            var categories = await blogService.GetBlogAllPostCategoryNames();

            var archive = await blogService.GetBlogArchive();

            var model = new BlogHomeViewModel()
            {
                Categories = categories,
                Posts = blogPostsPageingList,
                Archive = archive,
                Sorting = new SortPostViewModel() { Category = blogPostCategory, Month = sortMonth }
            };

            return model;
        }

        public async Task<ForumHomeViewModel> GetForumHomePagedModel(int pageNumber, string sortCategory = null)
        {
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            int pageSize = PageConstants.ForumHomePageSize;

            var (postsServiceModels, totalPostsCount) = await forumService.GetAllSortPostsForPageing(pageNumber, pageSize, sortCategory);

            var postsViewModel = mapper.Map<List<PostViewModel>>(postsServiceModels);

            var postsPageingList = new PagingList<PostViewModel>(postsViewModel, totalPostsCount, pageNumber, pageSize);

            var categories = await forumService.GetAllPostCategoryNames();

            var archive = await forumService.GetArchive();

            var model = new ForumHomeViewModel()
            {
                Categories = categories,
                Posts = postsPageingList,
                Archive = archive,
                Sorting = new SortPostViewModel() { Category = sortCategory }
            };

            return model;
        }

        public async Task<ForumPostViewModel> GetForumPostPagedModel(Guid id, int commentPage)
        {
            if (commentPage == 0)
            {
                commentPage = 1;
            }

            int pageSize = PageConstants.ForumCommentPageSize;

            var post = await forumService.GetPostServiceModelById(id);

            var commentsPerPage = await forumService.GetCommentsPerPage(id, commentPage, pageSize);

            var commentsViewModels = mapper.Map<List<CommentViewModel>>(commentsPerPage);

            var commentPageingList = new PagingList<CommentViewModel>(commentsViewModels, post.ForumComments.Count, commentPage, pageSize);

            var forumPostViewModel = mapper.Map<ForumPostViewModel>(post);

            forumPostViewModel.Comments = commentPageingList;

            return forumPostViewModel;
        }

        public async Task<PagingList<ProductViewModel>> GetProductsPagedModel(int pageNumber)
        {
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            int pageSize = PageConstants.ProductAllPageSize;

            (var productsServiceModels, var totalCount) = await productService.GetAllProductsForPageing(pageNumber, pageSize);

            var allProductsViewModels = mapper.Map<List<ProductViewModel>>(productsServiceModels);

            var pageList = new PagingList<ProductViewModel>(allProductsViewModels, totalCount, pageNumber, pageSize);

            return pageList;
        }

        public async Task<RecipePostViewModel> GetRecipePostPagedModel(Guid id, int commentPage)
        {
            if (commentPage == 0)
            {
                commentPage = 1;
            }

            int pageSize = PageConstants.RecipeCommentPageSize;

            var serviceModel = await recipeService.GetRecipeForPost(id);

            var commentsServiceModel = await recipeService.GetRecipeCommentsPerPage(id, commentPage, pageSize);

            var commentsViewModel = mapper.Map<List<CommentViewModel>>(commentsServiceModel);

            var commentsPagingList = new PagingList<CommentViewModel>(commentsViewModel, serviceModel.Comments.Count(), commentPage, pageSize);

            var recipePostViewModel = mapper.Map<RecipePostViewModel>(serviceModel);

            recipePostViewModel.Comments = commentsPagingList;

            return recipePostViewModel;
        }

        public async Task<PagingViewModel> GetRecipesPagedModel(int pageNumber, PagingViewModel sort)
        {
            var pagedList = await GetRecipesPagedListWithSortParameters(pageNumber, sort.Sorting.DishType, sort.Sorting.Category, sort.Sorting.Date);

            return pagedList;
        }

        public async Task<PagingViewModel> GetRecipesPagedModel(int pageNumber, string dishType, string category, bool dateAsc)
        {
            var pagedList = await GetRecipesPagedListWithSortParameters(pageNumber, dishType, category, dateAsc);

            return pagedList;
        }







        private async Task<PagingViewModel> GetRecipesPagedListWithSortParameters(int pageNumber, string dishType, string category, bool dateAsc)
        {
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            int pageSize = PageConstants.RecipeAllPageSize;

            (var recipesServiceModels, var totalCount) = await recipeService.GetSortRecipesForPageing(pageNumber, pageSize, dishType, category, dateAsc);

            var recipesViewModel = mapper.Map<List<AllRecipeViewModel>>(recipesServiceModels);

            var pagingViewModel = new PagingViewModel()
            {
                PagedList = new PagingList<AllRecipeViewModel>(recipesViewModel, totalCount, pageNumber, pageSize),
                Sorting = new SortRecipeViewModel() { DishType = dishType, Category = category, Date = dateAsc },
                Categories = await categoryService.GetAllCategoryNames()
            };

            return pagingViewModel;
        }
    }
}

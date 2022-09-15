using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Blog;
using CookDelicious.Core.Models.Blog;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Core.Models.Sorting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Controllers
{
    public class BlogController : BaseController
    {
        private readonly IBlogService blogService;
        private readonly IMapper mapper;

        public BlogController(IBlogService blogService, IMapper mapper)
        {
            this.blogService = blogService;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Home(int pageNumber, string blogPostCategory = null, int? sortMonth = null)
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
                Sorting = new SortPostViewModel() { Category = blogPostCategory, Month = sortMonth}
            };

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> BlogPost([FromRoute] Guid Id)
        {
            var blogPost = await blogService.GetBlogPostServiceModelById(Id);

            var blogPostrViewModel = mapper.Map<BlogPostViewModel>(blogPost);

            return View(blogPostrViewModel);
        }
    }
}

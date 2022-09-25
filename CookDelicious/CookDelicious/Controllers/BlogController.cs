using AutoMapper;
using CookDelicious.Core.Contracts.Blog;
using CookDelicious.Core.Contracts.Pageing;
using CookDelicious.Core.Models.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Controllers
{
    public class BlogController : BaseController
    {
        private readonly IBlogService blogService;
        private readonly IMapper mapper;
        private readonly IPageingService pageingService;

        public BlogController(IBlogService blogService, 
            IMapper mapper,
            IPageingService pageingService)
        {
            this.blogService = blogService;
            this.mapper = mapper;
            this.pageingService = pageingService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Home(int pageNumber, string blogPostCategory = null, int? sortMonth = null)
        {
            var model = await pageingService.GetBlogHomePagedModel(pageNumber, blogPostCategory, sortMonth);

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

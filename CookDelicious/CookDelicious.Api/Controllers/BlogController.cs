using AutoMapper;
using CookDelicious.Api.Models;
using CookDelicious.Core.Contracts.Blog;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService blogService;
        private readonly IMapper mapper;

        public BlogController(IBlogService blogService,
            IMapper mapper)
        {
            this.blogService = blogService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get blog posts per page. Can sort blog posts by month(1 - 12) and category("Ресторанти", "Фаст фууд", "Храни и напитки", "Веган", "Събития и лайвстайл", "Некатегоризирани")
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(200, StatusCode = StatusCodes.Status200OK, Type = typeof(List<AllBlogPostResponseModel>))]
        public async Task<IActionResult> GetAllBlogPosts(AllBlogPostRequestModel model)
        {
            var blogPostsPagedModel = await blogService.GetAllSortBlogPostsForPageing(model.PageNumber, model.PostsPerPage, model.BlogPostCategory, model.SortMonth);

            var blogPosts = mapper.Map<List<AllBlogPostResponseModel>>(blogPostsPagedModel.Items);

            return Ok(blogPosts);
        }
    }
}

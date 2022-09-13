using AutoMapper;
using CookDelicious.Core.Contracts.Admin;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Areas.Admin.Controllers
{
    public class BlogController : BaseController
    {
        private readonly IBlogServiceAdmin blogService;
        private readonly IMapper mapper;

        public BlogController(IBlogServiceAdmin blogService, IMapper mapper)
        {
            this.blogService = blogService;
            this.mapper = mapper;
        }

        public IActionResult CreateBlogPost()
        {
            //var categories = await forumService.GetAllPostCategoryNames();

            //var model = new CreatePostViewModel()
            //{
            //    Categories = categories,
            //};

            //return View(model);
        }



        public IActionResult CreateBlogPostCategory()
        {
            return View();
        }
    }
}

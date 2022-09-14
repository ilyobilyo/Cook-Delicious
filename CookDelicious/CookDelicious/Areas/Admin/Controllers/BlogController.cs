﻿using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Models.Admin.Blog;
using CookDelicious.Core.Service.Models.InputServiceModels;
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

        public async Task<IActionResult> CreateBlogPost()
        {
            var categories = await blogService.GetAllBlogPostCategoryNames();

            var model = new CreateBlogPostViewModel()
            {
                Categories = categories,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogPost(CreateBlogPostViewModel model)
        {
            var inputModel = mapper.Map<CreateBlogPostInputModel>(model);

            var error = await blogService.CreateBlogPost(inputModel, User.Identity.Name);

            if (error != null)
            {
                ViewData[MessageConstant.ErrorMessage] = error.Messages;
            }
            else
            {
                ViewData[MessageConstant.SuccessMessage] = "Вие публикувахте поста успешно!";
            }

            var categories = await blogService.GetAllBlogPostCategoryNames();

            model.Categories = categories;

            return View(model);
        }

        public IActionResult CreateBlogPostCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogPostCategory(CreateBlogPostCategoryViewModel model)
        {
            var inputModel = mapper.Map<CreateBlogPostCategoryInputModel>(model);

            var error = await blogService.CreateBlogPostCategory(inputModel);

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

        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid Id)
        {
            var IsDeleted = await blogService.DeleteBlogPost(Id);

            if (!IsDeleted)
            {
                return BadRequest("Неуспешно изтриване!");
            }

            return Redirect("/Blog/Home");
        }
    }
}

using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Models.Admin.Category;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Models;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryServiceAdmin categoryService;
        private readonly IMapper mapper;

        public CategoryController(ICategoryServiceAdmin categoryService,
            IMapper mapper)
        {
            this.categoryService = categoryService;
            this.mapper = mapper;
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("_Error", new ErrorViewModel() { Messages = MessageConstant.RequiredName });
            }

            var serviceModel = mapper.Map<CreateCategoryInputModel>(model);

            var error = await categoryService.CreateCategory(serviceModel);

            if (error.Messages != null)
            {
                ViewData[MessageConstant.ErrorMessage] = error.Messages;
            }
            else
            {
                ViewData[MessageConstant.SuccessMessage] = MessageConstant.SuccessfulRecord;
            }

            return View();
        }

        public async Task<IActionResult> ManageCategories()
        {
            var categoriesServiceModel = await categoryService.GetAllCategories();

            var categoriesViewModel = mapper.Map<List<CategoryViewModel>>(categoriesServiceModel);

            return View(categoriesViewModel);
        }

        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            await categoryService.DeleteCategory(id);

            return Redirect("/Admin/Category/ManageCategories");
        }
    }
}

using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Models.Admin.Category;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryServiceAdmin categoryService;

        public CategoryController(ICategoryServiceAdmin categoryService)
        {
            this.categoryService = categoryService;
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCategoryViewModel model)
        {
            var error = await categoryService.CreateCategory(model);

            if (error.Messages != null)
            {
                ViewData[MessageConstant.ErrorMessage] = error.Messages;
            }
            else
            {
                ViewData[MessageConstant.SuccessMessage] = "Успешен запис";
            }

            return View();
        }
    }
}

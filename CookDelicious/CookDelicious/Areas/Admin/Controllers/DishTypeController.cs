using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Models.Admin.DishType;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Areas.Admin.Controllers
{
    public class DishTypeController : BaseController
    {
        private readonly IDishTypeService dishTypeService;

        public DishTypeController(IDishTypeService dishTypeService)
        {
            this.dishTypeService = dishTypeService;
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateDishTypeViewModel model)
        {
            var error = await dishTypeService.CreateDishType(model);

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

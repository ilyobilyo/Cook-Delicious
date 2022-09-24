using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Models.Admin.DishType;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Models;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Areas.Admin.Controllers
{
    public class DishTypeController : BaseController
    {
        private readonly IDishTypeServiceAdmin dishTypeService;
        private readonly IMapper mapper;

        public DishTypeController(IDishTypeServiceAdmin dishTypeService,
            IMapper mapper)
        {
            this.dishTypeService = dishTypeService;
            this.mapper = mapper;
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateDishTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("_Error", new ErrorViewModel() { Messages = MessageConstant.RequiredName });
            }

            var serviceModel = mapper.Map<CreateDishTypeInputModel>(model);

            var error = await dishTypeService.CreateDishType(serviceModel);

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

        public async Task<IActionResult> ManageDishTypes()
        {
            var categoriesServiceModel = await dishTypeService.GetAllDishTypes();

            var categoriesViewModel = mapper.Map<List<DishTypeViewModel>>(categoriesServiceModel);

            return View(categoriesViewModel);
        }

        public async Task<IActionResult> DeleteDishType([FromRoute] Guid id)
        {
            await dishTypeService.DeleteDishType(id);

            return Redirect("/Admin/DishType/ManageDishTypes");
        }
    }
}

using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Models.Admin.DishType;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
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
            var serviceModel = mapper.Map<CreateDishTypeInputModel>(model);

            var error = await dishTypeService.CreateDishType(serviceModel);

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

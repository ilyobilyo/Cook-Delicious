using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin.Product;
using CookDelicious.Core.Contracts.Product;
using CookDelicious.Core.Models.Admin.Product;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Core.Models.Product;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductServiceAdmin productServiceAdmin;
        private readonly IProductService productService;
        private readonly IMapper mapper;

        public ProductController(IProductServiceAdmin productServiceAdmin,
            IProductService productService,
            IMapper mapper)
        {
            this.productServiceAdmin = productServiceAdmin;
            this.productService = productService;
            this.mapper = mapper;
        }

        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel model)
        {
            var serviceModel = mapper.Map<CreateProductInputModel>(model);

            var errors = await productServiceAdmin.CreateProduct(serviceModel);

            if (errors.Count > 0)
            {
                ViewData[MessageConstant.ErrorMessage] = string.Join(Environment.NewLine, errors.Select(x => x.Messages));
            }
            else
            {
                ViewData[MessageConstant.SuccessMessage] = "Успешен запис";
            }

            return View();
        }

        public async Task<IActionResult> All(int pageNumber)
        {
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            int pageSize = 9;

            (var productsServiceModels, var totalCount) = await productService.GetAllProductsForPageing(pageNumber, pageSize);

            var allProductsViewModels = mapper.Map<List<AllProductViewModel>>(productsServiceModels);

            var pageList = new PagingList<AllProductViewModel>(allProductsViewModels, totalCount, pageNumber, pageSize);

            return View(pageList);
        }
    }
}

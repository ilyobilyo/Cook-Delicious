using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin.Product;
using CookDelicious.Core.Contracts.Product;
using CookDelicious.Core.Models.Admin.Product;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Core.Models.Product;
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

            if (errors.Messages != null)
            {
                ViewData[MessageConstant.ErrorMessage] = errors.Messages;
            }
            else
            {
                ViewData[MessageConstant.SuccessMessage] = MessageConstant.SuccessfulRecord;
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

            var allProductsViewModels = mapper.Map<List<ProductViewModel>>(productsServiceModels);

            var pageList = new PagingList<ProductViewModel>(allProductsViewModels, totalCount, pageNumber, pageSize);

            return View(pageList);
        }

        public async Task<IActionResult> ManageProducts()
        {
            var products = await productService.GetAllProducts();

            var productsViewModel = mapper.Map<List<ProductViewModel>>(products);

            return View(productsViewModel);
        }

        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            await productServiceAdmin.DeleteProduct(id);

            return Redirect("/");
        }
    }
}

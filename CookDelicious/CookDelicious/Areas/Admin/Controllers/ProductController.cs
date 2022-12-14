using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin.Product;
using CookDelicious.Core.Contracts.Pageing;
using CookDelicious.Core.Contracts.Product;
using CookDelicious.Core.Models.Admin.Product;
using CookDelicious.Core.Models.Product;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Models;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductServiceAdmin productServiceAdmin;
        private readonly IProductService productService;
        private readonly IMapper mapper;
        private readonly IPageingService pageingService;

        public ProductController(IProductServiceAdmin productServiceAdmin,
            IProductService productService,
            IMapper mapper,
             IPageingService pageingService)
        {
            this.productServiceAdmin = productServiceAdmin;
            this.productService = productService;
            this.mapper = mapper;
            this.pageingService = pageingService;
        }

        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("_Error", new ErrorViewModel() { Messages = RecipeConstants.AllFieldsAreRequired });
            }

            var serviceModel = mapper.Map<CreateProductInputModel>(model);

            var error = await productServiceAdmin.CreateProduct(serviceModel);

            if (error != null)
            {
                ViewData[MessageConstant.ErrorMessage] = error;
            }
            else
            {
                ViewData[MessageConstant.SuccessMessage] = MessageConstant.SuccessfulRecord;
            }

            return View();
        }

        public async Task<IActionResult> All(int pageNumber)
        {
            var pageList = await pageingService.GetProductsPagedModel(pageNumber);

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

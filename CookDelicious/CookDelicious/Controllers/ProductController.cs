using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Product;
using CookDelicious.Core.Models.Product;
using CookDelicious.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Controllers
{
    [Authorize(Roles = "Administrator, User")]
    public class ProductController : BaseController
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel model)
        {
            var errors = await productService.CreateProduct(model);

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
            var products = await productService.GetAllProducts(pageNumber);

            return View(products);
        }

    }
}

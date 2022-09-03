using AutoMapper;
using CookDelicious.Core.Contracts.Product;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Core.Models.Product;
using CookDelicious.Core.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Controllers
{
    [Authorize(Roles = "Administrator, User")]
    public class ProductController : BaseController
    {
        private readonly IProductService productService;
        private readonly IMapper mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            this.productService = productService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> All(int pageNumber)
        {
            var products = await productService.GetAllProducts(pageNumber);

            var model = mapper.Map<IEnumerable<AllProductViewModel>>(products);

            return View(model);
        }

    }
}

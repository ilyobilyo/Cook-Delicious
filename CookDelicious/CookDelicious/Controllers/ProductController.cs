using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Pageing;
using CookDelicious.Core.Contracts.Product;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Core.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Controllers
{
    [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.User}")]
    public class ProductController : BaseController
    {
        private readonly IProductService productService;
        private readonly IMapper mapper;
        private readonly IPageingService pageingService;

        public ProductController(IProductService productService,
            IMapper mapper,
            IPageingService pageingService)
        {
            this.productService = productService;
            this.mapper = mapper;
            this.pageingService = pageingService;
        }

        public async Task<IActionResult> All(int pageNumber)
        {
            var pageList = await pageingService.GetProductsPagedModel(pageNumber);

            return View(pageList);
        }

        public async Task<IActionResult> ProductPost([FromRoute] Guid id)
        {
            var product = await productService.GetProductById(id);

            var productViewModel = mapper.Map<ProductViewModel>(product);

            return View(productViewModel);
        }
    }
}

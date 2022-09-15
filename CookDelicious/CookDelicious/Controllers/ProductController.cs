using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Product;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Core.Models.Product;
using CookDelicious.Core.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Controllers
{
    [Authorize(Roles = $"{UserConstants.Roles.Administrator}, {UserConstants.Roles.User}")]
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
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            int pageSize = PageConstants.ProductAllPageSize;

            (var productsServiceModels,var totalCount) = await productService.GetAllProductsForPageing(pageNumber, pageSize);

            var allProductsViewModels = mapper.Map<List<AllProductViewModel>>(productsServiceModels);

            var pageList = new PagingList<AllProductViewModel>(allProductsViewModels, totalCount, pageNumber, pageSize);

            return View(pageList);
        }

    }
}

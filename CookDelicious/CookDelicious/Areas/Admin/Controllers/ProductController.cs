﻿using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin.Product;
using CookDelicious.Core.Contracts.Product;
using CookDelicious.Core.Models.Admin.Product;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductServiceAdmin productServiceAdmin;
        private readonly IProductService productService;

        public ProductController(IProductServiceAdmin productServiceAdmin, IProductService productService)
        {
            this.productServiceAdmin = productServiceAdmin;
            this.productService = productService;
        }

        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel model)
        {
            var errors = await productServiceAdmin.CreateProduct(model);

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
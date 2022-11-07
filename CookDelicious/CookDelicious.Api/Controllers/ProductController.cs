using CookDelicious.Core.Contracts.Admin.Product;
using CookDelicious.Core.Contracts.Product;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Api.Controllers
{
    [Route("Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IProductServiceAdmin productServiceAdmin;

        public ProductController(IProductService productService,
            IProductServiceAdmin productServiceAdimn)
        {
            this.productService = productService;
            this.productServiceAdmin = productServiceAdimn;
        }

        /// <summary>
        /// Get All Specific Products
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        [Produces("application/json")]
        [ProducesResponseType(200, StatusCode = StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductServiceModel>))]
        public async Task<IActionResult> GetAll()
        {
            var products = await productService.GetAllProducts();

            return Ok(products);
        }

        /// <summary>
        /// Create a Specific Product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [Produces("application/json")]
        [ProducesResponseType(200, StatusCode = StatusCodes.Status200OK, Type = typeof(CreateProductInputModel))]
        [ProducesResponseType(400, StatusCode = StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CreateProduct(CreateProductInputModel model)
        {
            var error = await productServiceAdmin.CreateProduct(model);

            if (error != null)
            {
                return BadRequest(error);
            }

            return Ok(model);
        }
    }
}

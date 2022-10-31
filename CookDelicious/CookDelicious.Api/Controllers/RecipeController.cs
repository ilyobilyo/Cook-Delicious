using AutoMapper;
using CookDelicious.Core.Api.Models;
using CookDelicious.Core.Contracts.Recipes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Api.Controllers
{
    [Route("/Recipe")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService recipeService;
        private readonly IMapper mapper;

        public RecipeController(IRecipeService recipeService,
            IMapper mapper)
        {
            this.recipeService = recipeService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get best recipes from Cook Delicious
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("BestRecipes/{count:int}")]
        [Produces("application/json")]
        [ProducesResponseType(200, StatusCode = StatusCodes.Status200OK, Type = typeof(List<BestRecipesResponseModel>))]
        public async Task<IActionResult> GetBestRecipes(int count)
        {
            var bestRecipesServiceModels = await recipeService.GetBestRecipes(count);

            var recipesResponseModels = mapper.Map<List<BestRecipesResponseModel>>(bestRecipesServiceModels);

            return Ok(recipesResponseModels);
        }

        /// <summary>
        /// Get last added recipes from Cook Delicious
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("LastAddedRecipes/{count:int}")]
        [Produces("application/json")]
        [ProducesResponseType(200, StatusCode = StatusCodes.Status200OK, Type = typeof(List<LastAddedRecipesResponseModel>))]
        public async Task<IActionResult> LastAddedRecipes(int count)
        {
            var lastAddedRecipesServiceModels = await recipeService.GetLastAddedRecipes(count);

            var recipesResponseModels = mapper.Map<List<LastAddedRecipesResponseModel>>(lastAddedRecipesServiceModels);

            return Ok(recipesResponseModels);
        }
    }
}

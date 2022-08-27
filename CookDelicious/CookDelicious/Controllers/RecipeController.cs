﻿using CookDelicious.Core.Contracts.Recipe;
using CookDelicious.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookDelicious.Controllers
{
    [Authorize(Roles = "Administrator, User")]
    public class RecipeController : BaseController
    {
        private readonly IRecipeService recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
        }

        public async Task<IActionResult> All()
        {
            return Ok();
        }

        public async Task<IActionResult> CreateRecipe()
        {
            return Ok();
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateRecipe()
        //{
        //    List<ErrorViewModel> errors = recipeService.CreateRecipe();
        //}
    }
}

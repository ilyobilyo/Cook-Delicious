using AutoMapper;
using CookDelicious.Core.Models.Recipe;
using CookDelicious.Core.Service.Models;
using CookDelicious.Infrasturcture.Models.Recipes;

namespace CookDelicious.Core.MapProfiles
{
    public class RecipeMapping : Profile
    {
        public RecipeMapping()
        {
            CreateMap<Recipe, RecipeServiceModel>();
            CreateMap<RecipeServiceModel, AllRecipeViewModel>();
            CreateMap<RecipeServiceModel, RecipePostViewModel>();
        }
    }
}

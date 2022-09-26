using CookDelicious.Core.Models.Recipe;
using CookDelicious.Core.View.Models.Recipe;

namespace CookDelicious.Core.View.Models.Home
{
    public class HomeViewModel
    {
        public HomeRecipeViewModel TopRecipe { get; set; }

        public IList<HomeRecipeViewModel> LastAddedRecipes { get; set; }

        public IList<AllRecipeViewModel> BestRecipes { get; set; }
    }
}

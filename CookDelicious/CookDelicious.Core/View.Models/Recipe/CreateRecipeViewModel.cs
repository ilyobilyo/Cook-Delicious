using CookDelicious.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace CookDelicious.Core.Models.Recipe
{
    public class CreateRecipeViewModel
    {
        public CreateRecipeViewModel()
        {
            Categories =  new List<string>();
        }

        [Required(ErrorMessage = RecipeConstants.ReqiredRecipeTitle)]
        [StringLength(200,MinimumLength =2, ErrorMessage = RecipeConstants.RecipeTitleLength)]
        public string Title { get; set; }

        public string AuthorId { get; set; }

        [Required(ErrorMessage = RecipeConstants.ReqiredRecipeImage)]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = RecipeConstants.ReqiredRecipeDescription)]
        public string Description { get; set; }

        [Required(ErrorMessage = RecipeConstants.ReqiredRecipeCookingTime)]
        public string CookingTime { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string DishType { get; set; }

        [Required(ErrorMessage = RecipeConstants.ReqiredRecipeProducts)]
        [StringLength(600, MinimumLength = 5, ErrorMessage = RecipeConstants.ReqiredRecipeProductsMinimumLength)]
        public string Products { get; set; }

        public IList<string> Categories { get; set; }
    }
}

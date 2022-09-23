using CookDelicious.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace CookDelicious.Core.Models.Admin.Product
{
    public class CreateProductViewModel
    {
        [Required(ErrorMessage = MessageConstant.RequiredName)]
        [StringLength(100, MinimumLength = 2, ErrorMessage = RecipeConstants.ProductNameLength)]
        public string Name { get; set; }

        [Required(ErrorMessage = RecipeConstants.RequiredProductType)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = RecipeConstants.ProductTypeLength)]
        public string Type { get; set; }

        [Required(ErrorMessage = RecipeConstants.ProductImageRequired)]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = RecipeConstants.ProductDescriptionRequired)]
        public string Description { get; set; }
    }
}

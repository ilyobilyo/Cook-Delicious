using CookDelicious.Infrasturcture.Models.Recipes;
using System.ComponentModel.DataAnnotations;

namespace CookDelicious.Infrasturcture.Models.Common
{
    public class RecipeProduct
    {
        public Guid RecipeId { get; set; }

        public Recipe Recipe { get; set; }

        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        [MaxLength(10)]
        [Required]
        public string Quantity { get; set; }
    }
}

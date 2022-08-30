using CookDelicious.Core.Models.Product;
using CookDelicious.Infrasturcture.Models.Identity;

namespace CookDelicious.Core.Models.Recipe
{
    public class RecipePostViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public ApplicationUser Author { get; set; }

        public string PublishedOn { get; set; }

        public string ImageUrl { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public string DishType { get; set; }

        public string CookingTime { get; set; }

        public IList<RecipeProductViewModel> Products { get; set; }
    }
}

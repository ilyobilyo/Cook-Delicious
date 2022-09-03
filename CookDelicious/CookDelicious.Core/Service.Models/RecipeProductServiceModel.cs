namespace CookDelicious.Core.Service.Models
{
    public class RecipeProductServiceModel
    {
        public Guid RecipeId { get; set; }

        public RecipeServiceModel Recipe { get; set; }

        public Guid ProductId { get; set; }

        public ProductServiceModel Product { get; set; }

        public string Quantity { get; set; }
    }
}

namespace CookDelicious.Core.Service.Models
{
    public class RatingServiceModel
    {
        public Guid Id { get; set; }

        public int RatingDigit { get; set; }

        public Guid RecipeId { get; set; }

        public RecipeServiceModel Recipe { get; set; }
    }
}

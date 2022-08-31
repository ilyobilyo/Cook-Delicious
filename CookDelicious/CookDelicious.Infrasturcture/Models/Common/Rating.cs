using CookDelicious.Infrasturcture.Models.Recipes;

namespace CookDelicious.Infrasturcture.Models.Common
{
    public class Rating
    {
        public Guid Id { get; set; }

        public int RatingDigit { get; set; }

        public Guid RecipeId { get; set; }

        public Recipe Recipe { get; set; }
    }
}

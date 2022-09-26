namespace CookDelicious.Core.Api.Models
{
    public class BestRecipesResponseModel
    {
        public Guid Id { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public DateTime PublishedOn { get; set; }

        public string Description { get; set; }

        public string CookingTime { get; set; }

        public Guid CategoryId { get; set; }

        public string Catrgory { get; set; }

        public string DishType { get; set; }

        public int RatingStars { get; set; }

        public List<RecipeProductResponseModel> RecipeProducts { get; set; }
    }
}

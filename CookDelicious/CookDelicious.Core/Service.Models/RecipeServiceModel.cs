namespace CookDelicious.Core.Service.Models
{
    public class RecipeServiceModel
    {
        public Guid Id { get; set; }

        public string AuthorId { get; set; }

        public UserServiceModel Author { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public DateTime PublishedOn { get; set; }

        public string Description { get; set; }

        public string CookingTime { get; set; }

        public Guid CategoryId { get; set; }

        public CategoryServiceModel Catrgory { get; set; }

        public DishTypeServiceModel DishType { get; set; }

        public int RatingStars { get; set; }

        public bool? IsDeleted { get; set; }

        public IEnumerable<RatingServiceModel> Ratings { get; set; }

        public IEnumerable<RecipeProductServiceModel> RecipeProducts { get; set; }

        public IEnumerable<RecipeCommentServiceModel> Comments { get; set; }

        public Dictionary<string, string> Sorting { get; set; }
    }
}

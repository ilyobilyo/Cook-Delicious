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

        public CategoryServiceModel Category { get; set; }

        public Guid DishTypeId { get; set; }

        public DishTypeServiceModel DishType { get; set; }

        public bool? IsDeleted { get; set; }

        public ICollection<RatingServiceModel> Ratings { get; set; }

        public ICollection<RecipeProductServiceModel> RecipeProducts { get; set; }

        public ICollection<RecipeCommentServiceModel> Comments { get; set; }
    }
}

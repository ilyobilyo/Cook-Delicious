namespace CookDelicious.Core.Service.Models
{
    public class RecipeCommentServiceModel
    {
        public Guid Id { get; set; }

        public string Content { get; set; }

        public DateTime PublishedOn { get; set; }

        public string AuthorId { get; set; }

        public UserServiceModel Author { get; set; }

        public Guid RecipeId { get; set; }

        public RecipeServiceModel Recipe { get; set; }

        public bool? IsDeleted { get; set; } = false;
    }
}

namespace CookDelicious.Core.Service.Models
{
    public class BlogPostServiceModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime PublishedOn { get; set; }

        public string Content { get; set; }

        public string AuthorId { get; set; }

        public UserBlogServiceModel Author { get; set; }

        public Guid BlogPostCategoryId { get; set; }

        public BlogPostCategoryServiceModel BlogPostCategory { get; set; }

        public bool? IsDeleted { get; set; } = false;
    }
}

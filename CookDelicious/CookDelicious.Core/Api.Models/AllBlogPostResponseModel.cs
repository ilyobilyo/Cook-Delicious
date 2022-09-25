namespace CookDelicious.Api.Models
{
    public class AllBlogPostResponseModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime PublishedOn { get; set; }

        public string Content { get; set; }

        public string Author { get; set; }

        public string BlogPostCategory { get; set; }

        public bool? IsDeleted { get; set; } = false;
    }
}

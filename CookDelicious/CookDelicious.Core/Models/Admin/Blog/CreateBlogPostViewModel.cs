namespace CookDelicious.Core.Models.Admin.Blog
{
    public class CreateBlogPostViewModel
    {
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public DateTime PublishedOn { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public IList<string> Categories { get; set; }
    }
}

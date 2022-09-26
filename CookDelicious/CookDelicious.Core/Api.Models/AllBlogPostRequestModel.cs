using System.ComponentModel;

namespace CookDelicious.Core.Api.Models
{
    public class AllBlogPostRequestModel
    {
        public int PageNumber { get; set; }

        public int PostsPerPage { get; set; }

        [DefaultValue(null)]
        public string? BlogPostCategory { get; set; }

        [DefaultValue(null)]
        public int? SortMonth { get; set; }
    }
}

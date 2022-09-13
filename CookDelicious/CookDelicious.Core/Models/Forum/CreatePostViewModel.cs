using CookDelicious.Infrasturcture.Models.Forum;

namespace CookDelicious.Core.Models.Forum
{
    public class CreatePostViewModel
    {
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public DateTime PublishedOn { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public IList<string> Categories { get; set; }
    }
}

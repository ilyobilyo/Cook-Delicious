using CookDelicious.Core.Models.Paiging;

namespace CookDelicious.Core.Models.Blog
{
    public class BlogHomeViewModel
    {
        public IList<string> Categories { get; set; }

        public IList<string> Archive { get; set; }

        public PagingList<BlogPostViewModel> Posts { get; set; }
    }
}

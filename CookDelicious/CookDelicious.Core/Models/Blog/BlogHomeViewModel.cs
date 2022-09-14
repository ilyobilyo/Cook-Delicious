using CookDelicious.Core.Models.Paiging;
using CookDelicious.Core.Models.Sorting;

namespace CookDelicious.Core.Models.Blog
{
    public class BlogHomeViewModel
    {
        public IList<string> Categories { get; set; }

        public IList<string> Archive { get; set; }

        public PagingList<BlogPostViewModel> Posts { get; set; }

        public SortPostViewModel Sorting { get; set; }
    }
}

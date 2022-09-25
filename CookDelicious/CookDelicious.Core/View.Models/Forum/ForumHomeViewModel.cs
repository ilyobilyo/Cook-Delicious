using CookDelicious.Core.Models.Paiging;
using CookDelicious.Core.Models.Sorting;

namespace CookDelicious.Core.Models.Forum
{
    public class ForumHomeViewModel
    {
        public IList<string> Categories { get; set; }

        public IList<string> Archive { get; set; }

        public PagingList<PostViewModel> Posts { get; set; }

        public SortPostViewModel Sorting { get; set; }
    }
}

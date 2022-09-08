using CookDelicious.Core.Models.Paiging;

namespace CookDelicious.Core.Models.Forum
{
    public class ForumHomeViewModel
    {
        public IList<string> Categories { get; set; }

        public IList<string> Archive { get; set; }

        public PagingList<AllForumPostViewModel> Posts { get; set; }
    }
}

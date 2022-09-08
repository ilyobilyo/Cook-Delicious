using CookDelicious.Core.Models.Comments;
using CookDelicious.Core.Models.Paiging;

namespace CookDelicious.Core.Models.Forum
{
    public class ForumPostViewModel : AllForumPostViewModel
    {
        public PagingList<CommentViewModel> Comments { get; set; }

        public CommentViewModel Comment { get; set; }

    }
}

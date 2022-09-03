using CookDelicious.Core.Models.Comments;

namespace CookDelicious.Core.Models.Admin.Comments
{
    public class AdminCommentViewModel : CommentViewModel
    {
        public bool? IsDeleted { get; set; }
    }
}

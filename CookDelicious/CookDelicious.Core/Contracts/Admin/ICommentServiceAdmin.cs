using CookDelicious.Core.Models.Admin.Comments;
using CookDelicious.Core.Models.Paiging;

namespace CookDelicious.Core.Contracts.Admin
{
    public interface ICommentServiceAdmin
    {
        Task<PagingList<AdminCommentViewModel>> GetAllComments(int pageNumber);
    }
}

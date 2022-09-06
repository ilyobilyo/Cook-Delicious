using CookDelicious.Core.Models.Comments;
using CookDelicious.Core.Service.Models.InputServiceModels;

namespace CookDelicious.Core.Contracts.Comments
{
    public interface ICommentService
    {
        Task<bool> PostCommentForPost(Guid id, CommentViewModel model);
        Task<bool> DeletePostComment(Guid id);
        Task<bool> PostCommentForRecipe(Guid id, PostRecipeCommentInputModel model);
        Task<bool> DeleteRecipeComment(Guid id);
    }
}

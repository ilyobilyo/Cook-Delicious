using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Models;

namespace CookDelicious.Core.Contracts.Comments
{
    public interface ICommentService
    {
        Task<ErrorViewModel> PostCommentForPost(Guid id, PostCommentInputModel model);
        Task<bool> DeletePostComment(Guid id);
        Task<ErrorViewModel> PostCommentForRecipe(Guid id, PostCommentInputModel model);
        Task<bool> DeleteRecipeComment(Guid id);
    }
}

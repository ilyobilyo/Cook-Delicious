using CookDelicious.Core.Service.Models;

namespace CookDelicious.Core.Contracts.Admin
{
    public interface ICommentServiceAdmin
    {
        Task<IEnumerable<RecipeCommentServiceModel>> GetUserRecipeComments(string userId);
        Task<bool> DeleteRecipeComment(Guid id);
        Task<bool> DeleteForumComment(Guid id);
        Task<IEnumerable<ForumCommentServiceModel>> GetUserForumComments(string id);
    }
}

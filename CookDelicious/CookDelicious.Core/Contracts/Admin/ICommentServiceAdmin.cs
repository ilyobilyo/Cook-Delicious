using CookDelicious.Core.Service.Models;

namespace CookDelicious.Core.Contracts.Admin
{
    public interface ICommentServiceAdmin
    {
        Task<IEnumerable<RecipeCommentServiceModel>> GetRecipeComments(string userId);
        Task DeleteRecipeComment(Guid id);
        Task DeleteForumComment(Guid id);
        Task<IEnumerable<ForumCommentServiceModel>> GetForumComments(string id);
    }
}

using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Models;

namespace CookDelicious.Core.Contracts.Admin
{
    public interface IForumServiceAdmin
    {
        Task<ErrorViewModel> CreatePostCategory(CreatePostCategoryInputModel model);
        Task<IEnumerable<ForumPostServiceModel>> GetAllUserPosts(string id);
        Task<bool> DeletePost(Guid id);
        Task<IEnumerable<PostCategoryServiceModel>> GetAllForumPostCategories();
        Task<bool> DeleteForumPostCategory(Guid id);
    }
}

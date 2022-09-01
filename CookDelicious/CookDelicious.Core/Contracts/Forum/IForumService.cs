using CookDelicious.Core.Models.Forum;
using CookDelicious.Models;

namespace CookDelicious.Core.Contracts.Forum
{
    public interface IForumService
    {
        Task<IList<string>> GetAllPostCategoryNames();
        Task<ErrorViewModel> CreatePost(CreatePostViewModel model, string name);
        Task<IList<ForumPostViewModel>> GetAllPosts();
        Task<IList<string>> GetArchive();
        Task<ForumPostViewModel> GetPostById(Guid id);
    }
}

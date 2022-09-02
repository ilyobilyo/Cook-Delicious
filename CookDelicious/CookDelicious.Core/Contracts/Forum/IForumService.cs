using CookDelicious.Core.Models.Forum;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Infrasturcture.Models.Forum;
using CookDelicious.Models;

namespace CookDelicious.Core.Contracts.Forum
{
    public interface IForumService
    {
        Task<IList<string>> GetAllPostCategoryNames();
        Task<ErrorViewModel> CreatePost(CreatePostViewModel model, string name);
        Task<PagingList<ForumPostViewModel>> GetAllPosts(int pageNumber);
        Task<IList<string>> GetArchive();
        Task<ForumPostViewModel> GetPostById(Guid id, int commentPage);
        Task<bool> DeletePost(Guid id);
        public Task<ForumPost> GetById(Guid id);
    }
}

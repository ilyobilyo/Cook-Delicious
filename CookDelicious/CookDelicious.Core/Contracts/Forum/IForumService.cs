using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Infrasturcture.Models.Forum;
using CookDelicious.Models;

namespace CookDelicious.Core.Contracts.Forum
{
    public interface IForumService
    {
        Task<IList<string>> GetAllPostCategoryNames();
        Task<ErrorViewModel> CreatePost(CreateForumPostInputModel model, string name);
        Task<PagedListServiceModel<ForumPostServiceModel>> GetAllSortPostsForPageing(int pageNumber, int pageSize, string sortCategory);
        Task<IList<string>> GetArchive();
        Task<ForumPostServiceModel> GetPostServiceModelById(Guid id);
        Task<bool> DeletePost(Guid id);
        public Task<ForumPost> GetById(Guid id);
        Task<IEnumerable<ForumCommentServiceModel>> GetCommentsPerPage(Guid id, int commentPage, int pageSize);
    }
}

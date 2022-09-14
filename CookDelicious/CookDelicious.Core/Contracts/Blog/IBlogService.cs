using CookDelicious.Core.Service.Models;

namespace CookDelicious.Core.Contracts.Blog
{
    public interface IBlogService
    {
        Task<(IEnumerable<BlogPostServiceModel>, int)> GetAllSortBlogPostsForPageing(int pageNumber, int pageSize, string blogPostCategory, int? sortMonth);
        Task<IList<string>> GetBlogAllPostCategoryNames();
        Task<IList<string>> GetBlogArchive();
        Task<BlogPostServiceModel> GetBlogPostServiceModelById(Guid id);
    }
}

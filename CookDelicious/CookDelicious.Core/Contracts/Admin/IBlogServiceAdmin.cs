using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Models;

namespace CookDelicious.Core.Contracts.Admin
{
    public interface IBlogServiceAdmin
    {
        Task<ErrorViewModel> CreateBlogPostCategory(CreateBlogPostCategoryInputModel inputModel);
        Task<IList<string>> GetAllBlogPostCategoryNames();
        Task<ErrorViewModel> CreateBlogPost(CreateBlogPostInputModel inputModel, string name);
        Task<bool> DeleteBlogPost(Guid id);
    }
}

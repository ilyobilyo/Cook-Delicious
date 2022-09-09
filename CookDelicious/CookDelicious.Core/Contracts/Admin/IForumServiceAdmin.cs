using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Models;

namespace CookDelicious.Core.Contracts.Admin
{
    public interface IForumServiceAdmin
    {
        Task<ErrorViewModel> CreatePostCategory(CreatePostCategoryInputModel model);
    }
}

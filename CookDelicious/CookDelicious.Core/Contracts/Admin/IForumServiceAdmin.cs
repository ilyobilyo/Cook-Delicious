using CookDelicious.Core.Models.Admin.Forum;
using CookDelicious.Models;

namespace CookDelicious.Core.Contracts.Admin
{
    public interface IForumServiceAdmin
    {
        Task<ErrorViewModel> CreatePostCategory(CreatePostCategoryViewModel model);
    }
}

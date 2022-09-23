using CookDelicious.Core.Models.Blog;
using CookDelicious.Core.Models.Forum;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Core.Models.Product;
using CookDelicious.Core.Models.Recipe;

namespace CookDelicious.Core.Contracts.Pageing
{
    public interface IPageingService
    {
        Task<BlogHomeViewModel> GetBlogHomePagedModel(int pageNumber, string blogPostCategory = null, int? sortMonth = null);
        Task<ForumHomeViewModel> GetForumHomePagedModel(int pageNumber, string sortCategory = null);
        Task<ForumPostViewModel> GetForumPostPagedModel(Guid id, int commentPage);
        Task<PagingList<ProductViewModel>> GetProductsPagedModel(int pageNumber);
        Task<PagingViewModel> GetRecipesPagedModel(int pageNumber, PagingViewModel sort);
        Task<PagingViewModel> GetRecipesPagedModel(int pageNumber, string dishType, string category, bool dateAsc);
        Task<RecipePostViewModel> GetRecipePostPagedModel(Guid id, int commentPage);
    }
}

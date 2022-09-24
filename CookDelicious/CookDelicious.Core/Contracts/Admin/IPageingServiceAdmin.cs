using CookDelicious.Core.Models.Admin;
using CookDelicious.Core.Models.Paiging;

namespace CookDelicious.Core.Contracts.Admin
{
    public interface IPageingServiceAdmin
    {
        Task<PagingList<UserListViewModel>> GetAllUsersForManegment(int pageNumber);
    }
}

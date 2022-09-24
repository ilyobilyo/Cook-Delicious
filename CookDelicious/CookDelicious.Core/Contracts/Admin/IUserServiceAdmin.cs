using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Infrasturcture.Models.Identity;

namespace CookDelicious.Core.Contracts.Admin
{
    public interface IUserServiceAdmin
    {
        Task<PagedListServiceModel<UserServiceModel>> GetUsersPageingInManageUsers(int pageNumber, int pageSize);
        Task<ApplicationUser> GetUserByIdRoles(string id);
        Task<UserServiceModel> GetUserByIdEdit(string id);
        Task<bool> UpdateUser(UpdateUserInputModel model);
    }
}

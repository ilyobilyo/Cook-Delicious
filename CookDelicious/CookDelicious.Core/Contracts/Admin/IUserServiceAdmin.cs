using CookDelicious.Core.Models.Admin;
using CookDelicious.Infrasturcture.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookDelicious.Core.Contracts.Admin
{
    public interface IUserServiceAdmin
    {
        Task<IEnumerable<UserListViewModel>> GetUsersInManageUsers(int pageNumber);
        Task<ApplicationUser> GetUserByIdRoles(string id);
        Task<UserEditViewModel> GetUserByIdEdit(string id);
        Task<bool> UpdateUser(UserEditViewModel model);
    }
}

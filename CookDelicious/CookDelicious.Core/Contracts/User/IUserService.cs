using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Infrasturcture.Models.Identity;

namespace CookDelicious.Core.Contracts.User
{
    public interface IUserService
    {
        Task<bool> UpdateUser(UserEditProfileInputModel model);
        Task<UserServiceModel> GetUserByUsername(string author);
        Task<ApplicationUser> GetApplicationUserByUsername(string username);
    }
}

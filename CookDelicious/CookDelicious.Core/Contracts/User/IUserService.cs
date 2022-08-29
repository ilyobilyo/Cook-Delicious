using CookDelicious.Core.Models.User;
using CookDelicious.Infrasturcture.Models.Identity;

namespace CookDelicious.Core.Contracts.User
{
    public interface IUserService
    {
        Task<UserProfileViewModel> GetUserProfile(string userName);
        Task<UserEditProfileViewModel> GetUserProfileEdit(string userName);
        Task<bool> UpdateUser(UserEditProfileViewModel model);
        Task<ApplicationUser> GetUserByUsername(string author);
    }
}

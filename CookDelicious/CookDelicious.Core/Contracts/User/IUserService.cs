using CookDelicious.Core.Models.User;

namespace CookDelicious.Core.Contracts.User
{
    public interface IUserService
    {
        Task<UserProfileViewModel> GetUserProfile(string userName);
        Task<UserEditProfileViewModel> GetUserProfileEdit(string userName);
        Task<bool> UpdateUser(UserEditProfileViewModel model);
    }
}

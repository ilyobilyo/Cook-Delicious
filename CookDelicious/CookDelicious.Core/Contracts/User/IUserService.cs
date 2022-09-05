using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;

namespace CookDelicious.Core.Contracts.User
{
    public interface IUserService
    {
        Task<bool> UpdateUser(UserEditProfileInputModel model);
        Task<UserServiceModel> GetUserByUsername(string author);
    }
}

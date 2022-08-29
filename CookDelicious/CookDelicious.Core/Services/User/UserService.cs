using CookDelicious.Core.Contracts.User;
using CookDelicious.Core.Models.User;
using CookDelicious.Infrasturcture.Models.Identity;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CookDelicious.Core.Services.User
{
    public class UserService : IUserService
    {
        private readonly IApplicationDbRepository repo;

        public UserService(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task<ApplicationUser> GetUserByUsername(string author)
        {
            return await repo.All<ApplicationUser>()
                .Where(x => x.UserName == author)
                .FirstOrDefaultAsync();
        }

        public async Task<UserProfileViewModel> GetUserProfile(string userName)
        {
            return await repo.All<ApplicationUser>()
                .Where(x => x.UserName == userName)
                .Select(x => new UserProfileViewModel()
                {
                    Id = x.Id,
                    Age = x.Age,
                    ImageUrl = x.ImageUrl,
                    Email = x.Email,
                    Town = x.Town,
                    Username = x.UserName,
                    Address = x.Address,
                    CurrentJob = x.Job,
                    FirstName = x.FirsName,
                    LastName = x.LastName,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<UserEditProfileViewModel> GetUserProfileEdit(string userName)
        {
            return await repo.All<ApplicationUser>()
                .Where(x => x.UserName == userName)
                .Select(x => new UserEditProfileViewModel()
                {
                    Id = x.Id,
                    Address = x.Address,
                    Age = x.Age,
                    CurrentJob = x.Job,
                    Email = x.Email,
                    FirstName = x.FirsName,
                    ImageUrl = x.ImageUrl,
                    LastName = x.LastName,
                    Town = x.Town,
                    Username = x.UserName,
                })
                .FirstOrDefaultAsync();
        }


        public async Task<bool> UpdateUser(UserEditProfileViewModel model)
        {
            bool result = false;

            var user = await repo.GetByIdAsync<ApplicationUser>(model.Id);

            if (user != null)
            {
                user.UserName = model.Username;

                user.FirsName = model.FirstName;

                user.LastName = model.LastName;

                user.Town = model.Town;

                user.Email = model.Email;

                user.Job = model.CurrentJob;

                user.ImageUrl = model.ImageUrl;

                user.Age = model.Age;

                user.Address = model.Address;

                await repo.SaveChangesAsync();

                result = true;
            }

            return result;
        }
    }
}

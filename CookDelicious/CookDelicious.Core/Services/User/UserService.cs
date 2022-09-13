using AutoMapper;
using CookDelicious.Core.Contracts.User;
using CookDelicious.Core.Models.User;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Infrasturcture.Models.Identity;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CookDelicious.Core.Services.User
{
    public class UserService : IUserService
    {
        private readonly IApplicationDbRepository repo;
        private readonly IMapper mapper;

        public UserService(IApplicationDbRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<ApplicationUser> GetApplicationUserByUsername(string username)
        {
            return await repo.All<ApplicationUser>()
                .Where(x => x.UserName == username)
                .FirstOrDefaultAsync();
        }

        public async Task<UserServiceModel> GetUserByUsername(string author)
        {
            var user = await repo.All<ApplicationUser>()
                .Include(x => x.Recipes)
                .Where(x => x.UserName == author)
                .FirstOrDefaultAsync();

            return mapper.Map<UserServiceModel>(user);
        }
        public async Task<bool> UpdateUser(UserEditProfileInputModel model)
        {
            bool result = false;

            var user = await repo.GetByIdAsync<ApplicationUser>(model.Id);

            if (user != null)
            {
                user.UserName = model.Username;

                user.FirstName = model.FirstName;

                user.LastName = model.LastName;

                user.Town = model.Town;

                user.Email = model.Email;

                user.Job = model.Job;

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

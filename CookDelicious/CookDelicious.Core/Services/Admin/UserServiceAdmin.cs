using AutoMapper;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Infrasturcture.Models.Identity;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CookDelicious.Core.Services.Admin
{
    public class UserServiceAdmin : IUserServiceAdmin
    {
        private readonly IApplicationDbRepository repo;
        private readonly IMapper mapper;

        public UserServiceAdmin(IApplicationDbRepository repo,
            IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<UserServiceModel> GetUserByIdEdit(string id)
        {
            var user = await repo.GetByIdAsync<ApplicationUser>(id);

            return mapper.Map<UserServiceModel>(user);
        }

        public async Task<ApplicationUser> GetUserByIdRoles(string id)
        {
            return await repo.GetByIdAsync<ApplicationUser>(id);
        }

        public async Task<(IEnumerable<UserServiceModel>, int)> GetUsersPageingInManageUsers(int pageNumber, int pageSize)
        {
            var totalCount = await repo.All<ApplicationUser>()
                 .CountAsync();

            var users = await repo.All<ApplicationUser>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var usersServiceModels = mapper.Map<IEnumerable<UserServiceModel>>(users);

            return (usersServiceModels, totalCount);
        }

        public async Task<bool> UpdateUser(UpdateUserInputModel model)
        {
            bool result = false;

            var user = await repo.GetByIdAsync<ApplicationUser>(model.Id);

            if (user != null)
            {
                user.UserName = model.Username;

                await repo.SaveChangesAsync();

                result = true;
            }

            return result;
        }
    }
}

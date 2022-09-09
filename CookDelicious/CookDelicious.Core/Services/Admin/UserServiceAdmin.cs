﻿using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Models.Admin;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Infrasturcture.Models.Identity;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookDelicious.Core.Services.Admin
{
    public class UserServiceAdmin : IUserServiceAdmin
    {
        private readonly IApplicationDbRepository repo;

        public UserServiceAdmin(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task<UserEditViewModel> GetUserByIdEdit(string id)
        {
            var user = await repo.GetByIdAsync<ApplicationUser>(id);

            return new UserEditViewModel()
            {
                Id = user.Id,
                Username = user.UserName
            };
        }

        public async Task<ApplicationUser> GetUserByIdRoles(string id)
        {
            return await repo.GetByIdAsync<ApplicationUser>(id);
        }

        public async Task<IEnumerable<UserListViewModel>> GetUsersInManageUsers(int pageNumber)
        {

            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            int pageSize = 2;

            return await PagingList<UserListViewModel>.CreateAsync(repo.All<ApplicationUser>()
                .Select(x => new UserListViewModel()
                {
                    Email = x.Email,
                    Id = x.Id,
                    Username = x.UserName
                }),
                pageNumber,
                pageSize);
        }

        public async Task<bool> UpdateUser(UserEditViewModel model)
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

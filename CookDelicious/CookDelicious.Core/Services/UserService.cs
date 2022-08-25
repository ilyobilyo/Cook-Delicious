using CookDelicious.Core.Contracts;
using CookDelicious.Core.Models.Admin;
using CookDelicious.Infrasturcture.Models.Identity;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookDelicious.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationDbRepository repo;

        public UserService(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IEnumerable<UserListViewModel>> GetUsers()
        {
            return await repo.All<ApplicationUser>()
                .Select(x => new UserListViewModel()
                {
                    Id = x.Id,
                    Email = x.Email,
                    Username = x.UserName
                })
                .ToListAsync();
        }
    }
}

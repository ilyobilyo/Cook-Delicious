using CookDelicious.Core.Contracts;
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

namespace CookDelicious.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationDbRepository repo;

        public UserService(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IEnumerable<UserListViewModel>> GetUsers(int pageNumber)
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
    }
}

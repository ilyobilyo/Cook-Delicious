using CookDelicious.Core.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookDelicious.Core.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserListViewModel>> GetUsers(int pageNumber);
    }
}

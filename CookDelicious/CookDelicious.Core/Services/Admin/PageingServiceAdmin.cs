using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Models.Admin;
using CookDelicious.Core.Models.Paiging;

namespace CookDelicious.Core.Services.Admin
{
    public class PageingServiceAdmin : IPageingServiceAdmin
    {
        private readonly IUserServiceAdmin userService;
        private readonly IMapper mapper;

        public PageingServiceAdmin(IUserServiceAdmin userService,
            IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task<PagingList<UserListViewModel>> GetAllUsersForManegment(int pageNumber)
        {
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            int pageSize = PageConstants.ManageUsersPageSize;

            var usersPagedListServiceModels = await userService.GetUsersPageingInManageUsers(pageNumber, pageSize);

            var usersViewModel = mapper.Map<List<UserListViewModel>>(usersPagedListServiceModels.Items);

            var pageingList = new PagingList<UserListViewModel>(usersViewModel, usersPagedListServiceModels.TotalCount, pageNumber, pageSize);

            return pageingList;
        }
    }
}

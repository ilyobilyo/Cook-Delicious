using AutoMapper;
using CookDelicious.Core.Models.Admin;
using CookDelicious.Core.Models.User;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Infrasturcture.Models.Identity;

namespace CookDelicious.Core.MapProfiles
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<ApplicationUser, UserServiceModel>();
            CreateMap<UserServiceModel, UserListViewModel>();
            CreateMap<UserServiceModel, UserEditViewModel>();
            CreateMap<UserEditViewModel, UserServiceModel>();
            CreateMap<ApplicationUser, UserRolesViewModel>();
            CreateMap<ApplicationUser, UserForumServiceModel>();
            CreateMap<UserServiceModel, UserForumServiceModel>();
            CreateMap<UserServiceModel, ApplicationUser>();
            CreateMap<UserServiceModel, UserProfileViewModel>();
            CreateMap<UserServiceModel, UserEditProfileViewModel>();
            CreateMap<UserEditProfileViewModel, UserEditProfileInputModel>();
            CreateMap<UserEditViewModel, UpdateUserInputModel>();
        }
    }
}

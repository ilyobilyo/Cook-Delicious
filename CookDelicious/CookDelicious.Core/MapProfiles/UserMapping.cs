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
            CreateMap<ApplicationUser, UserBlogServiceModel>();
            CreateMap<UserServiceModel, UserListViewModel>();
            CreateMap<UserServiceModel, UserEditViewModel>();
            CreateMap<UserEditViewModel, UserServiceModel>();
            CreateMap<ApplicationUser, UserRolesViewModel>();
            CreateMap<ApplicationUser, UserForumServiceModel>();
            CreateMap<UserServiceModel, UserForumServiceModel>();
            CreateMap<UserServiceModel, ApplicationUser>();
            CreateMap<UserServiceModel, UserProfileViewModel>()
                .ForMember(x => x.MyRecipes, y => y.MapFrom(s => s.Recipes));
            CreateMap<UserServiceModel, UserEditProfileViewModel>();
            CreateMap<UserEditProfileViewModel, UserEditProfileInputModel>();
            CreateMap<UserEditViewModel, UpdateUserInputModel>();
        }
    }
}

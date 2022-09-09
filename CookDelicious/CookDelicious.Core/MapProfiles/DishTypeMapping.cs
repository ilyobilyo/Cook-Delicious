using AutoMapper;
using CookDelicious.Core.Models.Admin.DishType;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Infrasturcture.Models.Common;

namespace CookDelicious.Core.MapProfiles
{
    public class DishTypeMapping : Profile
    {
        public DishTypeMapping()
        {
            CreateMap<DishType, DishTypeServiceModel>();
            CreateMap<CreateDishTypeViewModel, CreateDishTypeInputModel>();
        }
    }
}

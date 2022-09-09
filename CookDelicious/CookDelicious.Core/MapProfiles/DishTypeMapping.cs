using AutoMapper;
using CookDelicious.Core.Service.Models;
using CookDelicious.Infrasturcture.Models.Common;

namespace CookDelicious.Core.MapProfiles
{
    public class DishTypeMapping : Profile
    {
        public DishTypeMapping()
        {
            CreateMap<DishType, DishTypeServiceModel>();
        }
    }
}

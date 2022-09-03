using AutoMapper;
using CookDelicious.Core.Service.Models;
using CookDelicious.Infrasturcture.Models.Common;

namespace CookDelicious.Core.MapProfiles
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<Category, CategoryServiceModel>();
        }
    }
}

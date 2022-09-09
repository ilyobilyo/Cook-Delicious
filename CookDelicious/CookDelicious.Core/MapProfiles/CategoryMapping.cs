using AutoMapper;
using CookDelicious.Core.Models.Admin.Category;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Infrasturcture.Models.Common;

namespace CookDelicious.Core.MapProfiles
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<Category, CategoryServiceModel>();
            CreateMap<AddCategoryViewModel, CreateCategoryInputModel>();
        }
    }
}

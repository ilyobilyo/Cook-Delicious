using AutoMapper;
using CookDelicious.Core.Models.Sorting;
using CookDelicious.Core.Service.Models;

namespace CookDelicious.Core.MapProfiles
{
    public class SortMapping : Profile
    {
        public SortMapping()
        {
            CreateMap<SortViewModel, SortServiceModel>();
        }
    }
}

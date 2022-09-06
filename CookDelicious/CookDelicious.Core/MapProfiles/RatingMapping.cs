using AutoMapper;
using CookDelicious.Core.Models.Recipe;
using CookDelicious.Core.Service.Models;
using CookDelicious.Infrasturcture.Models.Common;

namespace CookDelicious.Core.MapProfiles
{
    public class RatingMapping : Profile
    {
        public RatingMapping()
        {
            CreateMap<RatingViewModel, RatingSetServiceModel>();
            CreateMap<Rating, RatingServiceModel>();
            CreateMap<RecipeServiceModel, RatingViewModel>()
                .ForMember(x => x.DatePublishedOn, y => y.MapFrom(s => s.PublishedOn.ToString("dd")))
                .ForMember(x => x.MonthPublishedOn, y => y.MapFrom(s => s.PublishedOn.ToString("MMMM")))
                .ForMember(x => x.YearPublishedOn, y => y.MapFrom(s => s.PublishedOn.ToString("yyyy")))
                .ForMember(x => x.RatingOneCheck, y => y.Ignore())
                .ForMember(x => x.RatingTwoCheck, y => y.Ignore())
                .ForMember(x => x.RatingFourCheck, y => y.Ignore())
                .ForMember(x => x.RatingFiveCheck, y => y.Ignore())
                .ForMember(x => x.RatingThreeCheck, y => y.Ignore());
            CreateMap<RatingServiceModel, RatingViewModel>();
        }
    }
}

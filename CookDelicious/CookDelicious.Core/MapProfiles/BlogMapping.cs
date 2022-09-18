using AutoMapper;
using CookDelicious.Core.Models.Admin.Blog;
using CookDelicious.Core.Models.Blog;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Infrasturcture.Models.Blog;

namespace CookDelicious.Core.MapProfiles
{
    public class BlogMapping : Profile
    {
        public BlogMapping()
        {
            CreateMap<BlogPost, BlogPostServiceModel>();
            CreateMap<BlogPostCategory, BlogPostCategoryServiceModel>();
            CreateMap<BlogPostServiceModel, BlogPostViewModel>()
                .ForMember(x => x.DatePublishedOn, y => y.MapFrom(s => s.PublishedOn.ToString("dd")))
                .ForMember(x => x.MonthPublishedOn, y => y.MapFrom(s => s.PublishedOn.ToString("MMMM")))
                .ForMember(x => x.YearPublishedOn, y => y.MapFrom(s => s.PublishedOn.ToString("yyyy")))
                .ForMember(x => x.AuthorName, y => y.MapFrom(s => s.Author.UserName))
                .ForMember(x => x.CategoryName, y => y.MapFrom(s => s.BlogPostCategory.Name));
            CreateMap<CreateBlogPostViewModel, CreateBlogPostInputModel>();
            CreateMap<CreateBlogPostCategoryViewModel, CreateBlogPostCategoryInputModel>();
            CreateMap<BlogPostCategoryServiceModel, BlogPostCategoryViewModel>();
        }
    }
}

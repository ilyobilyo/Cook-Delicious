using AutoMapper;
using CookDelicious.Core.Models.Admin.Comments;
using CookDelicious.Core.Models.Admin.Forum;
using CookDelicious.Core.Models.Comments;
using CookDelicious.Core.Models.Forum;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Infrasturcture.Models.Forum;

namespace CookDelicious.Core.MapProfiles
{
    public class ForumMapping : Profile
    {
        public ForumMapping()
        {
            CreateMap<ForumPost, ForumPostServiceModel>();
            CreateMap<ForumPostServiceModel, ForumPostViewModel>()
                .ForMember(x => x.DatePublishedOn, y => y.MapFrom(s => s.PublishedOn.ToString("dd")))
                .ForMember(x => x.MonthPublishedOn, y => y.MapFrom(s => s.PublishedOn.ToString("MMMM")))
                .ForMember(x => x.YearPublishedOn, y => y.MapFrom(s => s.PublishedOn.ToString("yyyy")))
                .ForMember(x => x.AuthorName, y => y.MapFrom(s => s.Author.UserName))
                .ForMember(x => x.CategoryName, y => y.MapFrom(s => s.PostCategory.Name))
                .ForMember(x => x.Comments, y => y.Ignore());
            CreateMap<ForumPostServiceModel, AllForumPostViewModel>()
                .ForMember(x => x.DatePublishedOn, y => y.MapFrom(s => s.PublishedOn.ToString("dd")))
                .ForMember(x => x.MonthPublishedOn, y => y.MapFrom(s => s.PublishedOn.ToString("MMMM")))
                .ForMember(x => x.YearPublishedOn, y => y.MapFrom(s => s.PublishedOn.ToString("yyyy")))
                .ForMember(x => x.AuthorName, y => y.MapFrom(s => s.Author.UserName))
                .ForMember(x => x.CategoryName, y => y.MapFrom(s => s.PostCategory.Name));
            CreateMap<PostCategory, PostCategoryServiceModel>();
            CreateMap<CreatePostCategoryViewModel, CreatePostCategoryInputModel>();
            CreateMap<ForumComment, ForumCommentServiceModel>();
            CreateMap<ForumCommentServiceModel, CommentViewModel>()
                .ForMember(x => x.AuthorName, y => y.MapFrom(s => s.Author.UserName));
            CreateMap<CreatePostViewModel, CreateForumPostInputModel>();
            CreateMap<CommentViewModel, PostCommentInputModel>();
            CreateMap<ForumCommentServiceModel, AdminCommentViewModel>()
                .ForMember(x => x.AuthorName, y => y.MapFrom(s => s.Author.UserName));
        }
    }
}

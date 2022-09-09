using AutoMapper;
using CookDelicious.Core.Models.Admin.Comments;
using CookDelicious.Core.Models.Comments;
using CookDelicious.Core.Models.Product;
using CookDelicious.Core.Models.Recipe;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Infrasturcture.Models.Recipes;

namespace CookDelicious.Core.MapProfiles
{
    public class RecipeMapping : Profile
    {
        public RecipeMapping()
        {
            CreateMap<Recipe, RecipeServiceModel>();
            CreateMap<RecipeServiceModel, AllRecipeViewModel>();
            CreateMap<RecipeServiceModel, RecipePostViewModel>()
                .ForMember(x => x.Category, y => y.MapFrom(s => s.Category.Name))
                .ForMember(x => x.DishType, y => y.MapFrom(s => s.DishType.Name))
                .ForMember(x => x.Comments, y => y.Ignore())
                .ForMember(x => x.PublishedOn, y => y.MapFrom(s => s.PublishedOn.ToString("dd MM yyyy")));
            CreateMap<CreateRecipeViewModel, CreateRecipeServiceModel>();
            CreateMap<CreateRecipeServiceModel, CreateRecipeViewModel>();
            CreateMap<RecipeComment, RecipeCommentServiceModel>();
            CreateMap<CommentViewModel, PostCommentInputModel>();
            CreateMap<RecipeCommentServiceModel, CommentViewModel>()
                .ForMember(x => x.AuthorName, y => y.MapFrom(s => s.Author.UserName));
            CreateMap<RecipeProductServiceModel, RecipeProductViewModel>()
                .ForMember(x => x.Product, y => y.MapFrom(s => s.Product.Name));
            CreateMap<RecipeCommentServiceModel, AdminCommentViewModel>()
                .ForMember(x => x.AuthorName, y => y.MapFrom(s => s.Author.UserName));
        }
    }
}

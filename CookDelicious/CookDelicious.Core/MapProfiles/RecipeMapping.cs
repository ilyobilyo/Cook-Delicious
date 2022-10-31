using AutoMapper;
using CookDelicious.Core.Api.Models;
using CookDelicious.Core.Models.Admin.Comments;
using CookDelicious.Core.Models.Admin.Recipe;
using CookDelicious.Core.Models.Comments;
using CookDelicious.Core.Models.Product;
using CookDelicious.Core.Models.Recipe;
using CookDelicious.Core.Models.User;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Core.View.Models.Recipe;
using CookDelicious.Infrasturcture.Models.Recipes;

namespace CookDelicious.Core.MapProfiles
{
    public class RecipeMapping : Profile
    {
        public RecipeMapping()
        {
            CreateMap<Recipe, RecipeServiceModel>()
                .ForMember(x => x.Sorting, y => y.Ignore());
            CreateMap<RecipeServiceModel, AllRecipeViewModel>();
            CreateMap<RecipeServiceModel, RecipePostViewModel>()
                .ForMember(x => x.Category, y => y.MapFrom(s => s.Catrgory.Name))
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
            CreateMap<RecipeServiceModel, MyRecipesViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.Title));
            CreateMap<RecipeServiceModel, ManageRecipeViewModel>()
                .ForMember(x => x.AuthorName, y => y.MapFrom(c => c.Author.UserName));
            CreateMap<RecipeServiceModel, HomeRecipeViewModel>();
            CreateMap<RecipeServiceModel, BestRecipesResponseModel>()
                .ForMember(x => x.AuthorName, y => y.MapFrom(s => s.Author.UserName))
                .ForMember(x => x.Catrgory, y => y.MapFrom(s => s.Catrgory.Name))
                .ForMember(x => x.DishType, y => y.MapFrom(s => s.DishType.Name));
            CreateMap<RecipeServiceModel, LastAddedRecipesResponseModel>()
                .ForMember(x => x.Category, y => y.MapFrom(s => s.Catrgory.Name));
        }
    }
}

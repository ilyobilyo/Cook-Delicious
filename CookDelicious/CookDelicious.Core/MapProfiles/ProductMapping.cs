using AutoMapper;
using CookDelicious.Core.Api.Models;
using CookDelicious.Core.Models.Admin.Product;
using CookDelicious.Core.Models.Product;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Infrasturcture.Models.Common;

namespace CookDelicious.Core.MapProfiles
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductServiceModel>();
            CreateMap<ProductServiceModel, ProductViewModel>();
            CreateMap<RecipeProduct, RecipeProductServiceModel>();
            CreateMap<CreateProductViewModel, CreateProductInputModel>();
            CreateMap<RecipeProductViewModel, CreateRecipeProductServiceModel>();
            CreateMap<RecipeProductServiceModel, RecipeProductResponseModel>()
                .ForMember(x => x.ProductName, y => y.MapFrom(s => s.Product.Name));
        }
    }
}

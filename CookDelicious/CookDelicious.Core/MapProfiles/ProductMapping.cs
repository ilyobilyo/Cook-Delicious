using AutoMapper;
using CookDelicious.Core.Models.Product;
using CookDelicious.Core.Service.Models;
using CookDelicious.Infrasturcture.Models.Common;

namespace CookDelicious.Core.MapProfiles
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductServiceModel>();
            CreateMap<ProductServiceModel, AllProductViewModel>();
            CreateMap<RecipeProduct, RecipeProductServiceModel>();
        }
    }
}

using CookDelicious.Core.Contracts.Product;
using CookDelicious.Core.Models.Product;
using CookDelicious.Infrasturcture.Repositories;
using CookDelicious.Infrasturcture.Models.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Models;

namespace CookDelicious.Core.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IApplicationDbRepository repo;

        public ProductService(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }


        public async Task<IEnumerable<AllProductViewModel>> GetAllProducts(int pageNumber)
        {
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            int pageSize = 9;

            return await PagingList<AllProductViewModel>.CreateAsync(repo.All<Product>()
                .Select(p => new AllProductViewModel
                {
                    Type = p.Type,
                    Name = p.Name,
                }),
                pageNumber,
                pageSize);
        }

    }
}

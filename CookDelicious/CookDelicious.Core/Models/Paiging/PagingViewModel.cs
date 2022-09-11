
using CookDelicious.Core.Models.Recipe;
using CookDelicious.Core.Service.Models;

namespace CookDelicious.Core.Models.Paiging
{
    public class RecipePagingViewModel
    {
        public PagingList<AllRecipeViewModel> PagedList { get; set; }

        public SortServiceModel Sorting { get; set; }
    }
}

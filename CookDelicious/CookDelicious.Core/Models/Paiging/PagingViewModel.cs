
using CookDelicious.Core.Models.Recipe;
using CookDelicious.Core.Models.Sorting;

namespace CookDelicious.Core.Models.Paiging
{
    public class RecipePagingViewModel
    {
        public PagingList<AllRecipeViewModel> PagedList { get; set; }

        public SortRecipeViewModel Sorting { get; set; }

        public IList<string> Categories { get; set; }
    }
}

using CookDelicious.Core.Models.Sorting;
using CookDelicious.Core.Service.Models;
using CookDelicious.Infrasturcture.Models.Common;
using System.ComponentModel;

namespace CookDelicious.Core.Models.Recipe
{
    public class AllRecipeViewModel
    {
        public AllRecipeViewModel()
        {
            Ratings = new List<RatingViewModel>();
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public ICollection<RatingViewModel> Ratings { get; set; }

        public int RatingStars { get; set; }

        public string ImageUrl { get; set; }
    }
}

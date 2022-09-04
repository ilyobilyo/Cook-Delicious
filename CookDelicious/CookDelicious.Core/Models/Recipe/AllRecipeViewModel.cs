using CookDelicious.Core.Service.Models;
using CookDelicious.Infrasturcture.Models.Common;
using System.ComponentModel;

namespace CookDelicious.Core.Models.Recipe
{
    public class AllRecipeViewModel
    {
        public AllRecipeViewModel()
        {
            Ratings = new List<RatingServiceModel>();
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public ICollection<RatingServiceModel> Ratings { get; set; }


        public int Rating
        {
            get
            {
                if (Ratings.Count > 0)
                {
                    return (int)Math.Round(Ratings.Average(x => x.RatingDigit)); ;
                }
                else
                {
                    return 0;
                }
            }
        }

        public string ImageUrl { get; set; }
    }
}

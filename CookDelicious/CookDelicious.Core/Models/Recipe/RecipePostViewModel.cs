using CookDelicious.Core.Models.Comments;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Core.Models.Product;
using CookDelicious.Core.Service.Models;
using CookDelicious.Infrasturcture.Models.Identity;

namespace CookDelicious.Core.Models.Recipe
{
    public class RecipePostViewModel
    {
        public RecipePostViewModel()
        {
            Ratings = new List<RatingServiceModel>();
        }

        public Guid Id { get; set; }

        public string Title { get; set; }

        public ApplicationUser Author { get; set; }

        public string PublishedOn { get; set; }

        public string ImageUrl { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public string DishType { get; set; }

        public string CookingTime { get; set; }

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

        public ICollection<RatingServiceModel> Ratings { get; set; }

        public IList<RecipeProductViewModel> Products { get; set; }

        public PagingList<CommentViewModel> Comments { get; set; }

        public CommentViewModel Comment { get; set; }
    }
}

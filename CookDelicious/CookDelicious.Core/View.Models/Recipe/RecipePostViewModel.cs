using CookDelicious.Core.Models.Comments;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Core.Models.Product;
using CookDelicious.Core.Service.Models;

namespace CookDelicious.Core.Models.Recipe
{
    public class RecipePostViewModel
    {
        public RecipePostViewModel()
        {
            Ratings = new List<RatingViewModel>();
        }

        public Guid Id { get; set; }

        public string Title { get; set; }

        public UserServiceModel Author { get; set; }

        public string PublishedOn { get; set; }

        public string ImageUrl { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public string DishType { get; set; }

        public string CookingTime { get; set; }

        public int RatingStars { get; set; }

        public ICollection<RatingViewModel> Ratings { get; set; }

        public IList<RecipeProductViewModel> RecipeProducts { get; set; }

        public PagingList<CommentViewModel> Comments { get; set; }

        public CommentViewModel Comment { get; set; }
    }
}

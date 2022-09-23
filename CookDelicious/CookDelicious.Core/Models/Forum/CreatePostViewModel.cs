using CookDelicious.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace CookDelicious.Core.Models.Forum
{
    public class CreatePostViewModel
    {
        [Required(ErrorMessage = PostsConstants.RequiredTitle)]
        [StringLength(100, MinimumLength = 2, ErrorMessage = PostsConstants.TitleLength)]
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public DateTime PublishedOn { get; set; }

        [Required(ErrorMessage = PostsConstants.RequiredContent)]
        public string Description { get; set; }

        public string Category { get; set; }

        public IList<string> Categories { get; set; }
    }
}

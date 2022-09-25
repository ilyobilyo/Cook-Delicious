using CookDelicious.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace CookDelicious.Core.Models.Comments
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = UserConstants.InvalidAuthor)]
        public string AuthorName { get; set; }

        [Required(ErrorMessage = CommentConstants.CommentContentRequired)]
        [StringLength(200, MinimumLength = 1, ErrorMessage = CommentConstants.CommentContentLength)]
        public string Content { get; set; }
    }
}

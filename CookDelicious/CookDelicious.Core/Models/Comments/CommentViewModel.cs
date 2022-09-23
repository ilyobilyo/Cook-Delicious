using CookDelicious.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace CookDelicious.Core.Models.Comments
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = UserConstants.InvalidAuthor)]
        public string AuthorName { get; set; }

        [Required(ErrorMessage = CommentConstants.CommentContentMinimumLength)]
        [StringLength(200, MinimumLength = 1, ErrorMessage = CommentConstants.CommentMaxLength)]
        public string Content { get; set; }
    }
}

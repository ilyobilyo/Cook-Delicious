using CookDelicious.Infrasturcture.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CookDelicious.Infrasturcture.Models.Forum
{
    public class ForumPost
    {
        public ForumPost()
        {
            ForumComments = new HashSet<ForumComment>();
        }

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public DateTime PublishedOn { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public ApplicationUser Author { get; set; }

        [Required]
        public Guid PostCategoryId { get; set; }

        [ForeignKey(nameof(PostCategoryId))]
        public PostCategory PostCategory { get; set; }

        public bool? IsDeleted { get; set; } = false;

        public ICollection<ForumComment> ForumComments { get; set; }
    }
}

using CookDelicious.Infrasturcture.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CookDelicious.Infrasturcture.Models.Forum
{
    public class ForumComment
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(450)]
        public string Content { get; set; }

        [Required]
        public DateTime PublishedOn { get; set; }

        [Required]
        public Guid ForumPostId { get; set; }

        [ForeignKey(nameof(ForumPostId))]
        public ForumPost ForumPost { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public ApplicationUser Author { get; set; }
    }
}

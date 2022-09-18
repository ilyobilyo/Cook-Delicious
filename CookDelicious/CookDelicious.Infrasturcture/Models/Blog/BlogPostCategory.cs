using System.ComponentModel.DataAnnotations;

namespace CookDelicious.Infrasturcture.Models.Blog
{
    public class BlogPostCategory
    {
        public BlogPostCategory()
        {
            BlogPosts = new HashSet<BlogPost>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public bool? IsDeleted { get; set; } = false;

        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}

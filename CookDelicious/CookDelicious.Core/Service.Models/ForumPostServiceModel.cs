namespace CookDelicious.Core.Service.Models
{
    public class ForumPostServiceModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime PublishedOn { get; set; }

        public string Content { get; set; }

        public string AuthorId { get; set; }

        public UserForumServiceModel Author { get; set; }

        public Guid PostCategoryId { get; set; }

        public PostCategoryServiceModel PostCategory { get; set; }

        public bool? IsDeleted { get; set; } = false;

        public ICollection<ForumCommentServiceModel> ForumComments { get; set; }
    }
}

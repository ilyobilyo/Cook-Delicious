namespace CookDelicious.Core.Service.Models
{
    public class ForumCommentServiceModel
    {
        public Guid Id { get; set; } 

        public string Content { get; set; }

        public DateTime PublishedOn { get; set; }

        public Guid ForumPostId { get; set; }

        public ForumPostServiceModel ForumPost { get; set; }

        public string AuthorId { get; set; }

        public UserForumServiceModel Author { get; set; }

        public bool? IsDeleted { get; set; } = false;
    }
}

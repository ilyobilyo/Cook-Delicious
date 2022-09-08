namespace CookDelicious.Core.Service.Models
{
    public class UserForumServiceModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? ImageUrl { get; set; }

        public int? Age { get; set; }

        public string? Town { get; set; }

        public string? Job { get; set; }

        public string? Address { get; set; }

        public ICollection<ForumPostServiceModel> ForumPosts { get; set; }

        public ICollection<ForumCommentServiceModel> ForumComments { get; set; }
    }
}

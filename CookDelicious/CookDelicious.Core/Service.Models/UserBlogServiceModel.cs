namespace CookDelicious.Core.Service.Models
{
    public class UserBlogServiceModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }

        public string Job { get; set; }

        public ICollection<BlogPostServiceModel> BlogPosts { get; set; }

    }
}

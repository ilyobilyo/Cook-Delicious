namespace CookDelicious.Infrasturcture.Models.Forum
{
    public class PostCategory
    {
        public PostCategory()
        {
            ForumPosts = new HashSet<ForumPost>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool? IsDeleted { get; set; } = false;

        public ICollection<ForumPost> ForumPosts { get; set; }
    }
}

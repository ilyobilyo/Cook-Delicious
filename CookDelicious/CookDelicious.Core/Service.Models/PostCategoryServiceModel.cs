namespace CookDelicious.Core.Service.Models
{
    public class PostCategoryServiceModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<ForumPostServiceModel> ForumPosts { get; set; }
    }
}

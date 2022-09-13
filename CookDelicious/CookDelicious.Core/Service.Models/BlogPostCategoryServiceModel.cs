namespace CookDelicious.Core.Service.Models
{
    public class BlogPostCategoryServiceModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<BlogPostServiceModel> BlogPosts { get; set; }
    }
}

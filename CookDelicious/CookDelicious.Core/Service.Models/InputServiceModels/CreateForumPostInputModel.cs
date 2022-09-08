namespace CookDelicious.Core.Service.Models.InputServiceModels
{
    public class CreateForumPostInputModel
    {
        public string Title { get; set; }

        public string AuthorId { get; set; }

        public string ImageUrl { get; set; }

        public DateTime PublishedOn { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public IList<string> Categories { get; set; }
    }
}

namespace CookDelicious.Core.Api.Models
{
    public class LastAddedRecipesResponseModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string CookingTime { get; set; }

        public DateTime PublishedOn { get; set; }

        public string AuthorId { get; set; }

        public string CategoryId { get; set; }

        public string Category { get; set; }

        public string ImageUrl { get; set; }
    }
}

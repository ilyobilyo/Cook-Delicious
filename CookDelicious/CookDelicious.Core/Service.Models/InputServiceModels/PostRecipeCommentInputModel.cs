namespace CookDelicious.Core.Service.Models.InputServiceModels
{
    public class PostCommentInputModel
    {
        public Guid Id { get; set; }

        public string AuthorName { get; set; }

        public string Content { get; set; }
    }
}

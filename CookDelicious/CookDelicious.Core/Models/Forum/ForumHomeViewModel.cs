namespace CookDelicious.Core.Models.Forum
{
    public class ForumHomeViewModel
    {
        public IList<string> Categories { get; set; }

        public IList<string> Archive { get; set; }

        public IList<ForumPostViewModel> Posts { get; set; }
    }
}

namespace CookDelicious.Core.Models.Forum
{
    public class AllForumPostViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Content { get; set; }

        public string DatePublishedOn { get; set; }

        public string MonthPublishedOn { get; set; }

        public string YearPublishedOn { get; set; }

        public string AuthorName { get; set; }

        public string CategoryName { get; set; }
    }
}

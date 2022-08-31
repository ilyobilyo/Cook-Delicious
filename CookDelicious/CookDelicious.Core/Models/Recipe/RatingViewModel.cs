namespace CookDelicious.Core.Models.Recipe
{
    public class RatingViewModel
    {
        public string Title { get; set; }

        public Guid Id { get; set; }

        public string ImageUrl { get; set; }

        public string DatePublishedOn { get; set; }

        public string MonthPublishedOn { get; set; }

        public string YearPublishedOn { get; set; }

        public bool RatingOneCheck { get; set; }

        public bool RatingTwoCheck { get; set; }

        public bool RatingThreeCheck { get; set; }

        public bool RatingFourCheck { get; set; }

        public bool RatingFiveCheck { get; set; }

    }
}

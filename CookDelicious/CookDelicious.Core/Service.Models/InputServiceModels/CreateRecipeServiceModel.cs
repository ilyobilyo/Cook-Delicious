namespace CookDelicious.Core.Service.Models.InputServiceModels
{
    public class CreateRecipeServiceModel
    {
        public CreateRecipeServiceModel()
        {
            Categories = new List<string>();
        }

        public string Title { get; set; }

        public string AuthorId { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public string CookingTime { get; set; }

        public string Category { get; set; }

        public string DishType { get; set; }

        public string Products { get; set; }

        public IList<string> Categories { get; set; }
    }
}

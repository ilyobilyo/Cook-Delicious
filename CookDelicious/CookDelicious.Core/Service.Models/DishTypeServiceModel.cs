namespace CookDelicious.Core.Service.Models
{
    public class DishTypeServiceModel
    {
        public Guid Id { get; set; } 

        public string Name { get; set; }

        public bool? IsDeleted { get; set; } = false;

        public ICollection<RecipeServiceModel> Recipes { get; set; }
    }
}

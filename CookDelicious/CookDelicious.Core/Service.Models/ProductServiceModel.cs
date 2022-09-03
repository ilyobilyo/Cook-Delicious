namespace CookDelicious.Core.Service.Models
{
    public class ProductServiceModel
    {
        public Guid Id { get; set; } 

        public string Name { get; set; }

        public string Type { get; set; }

        public bool? IsDeleted { get; set; } = false;

        public ICollection<RecipeProductServiceModel> RecipeProducts { get; set; }
    }
}

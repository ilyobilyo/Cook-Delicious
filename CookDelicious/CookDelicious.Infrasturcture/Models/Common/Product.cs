using System.ComponentModel.DataAnnotations;

namespace CookDelicious.Infrasturcture.Models.Common
{
    public class Product
    {
        public Product()
        {
            RecipeProducts = new HashSet<RecipeProduct>();
        }

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Type { get; set; }

        public string? ImageUrl { get; set; }

        public string? Description { get; set; }

        public bool? IsDeleted { get; set; } = false;

        public ICollection<RecipeProduct> RecipeProducts { get; set; }
    }
}

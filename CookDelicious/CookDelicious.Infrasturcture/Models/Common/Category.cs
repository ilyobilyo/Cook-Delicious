using CookDelicious.Infrasturcture.Models.Recipes;
using System.ComponentModel.DataAnnotations;

namespace CookDelicious.Infrasturcture.Models.Common
{
    public class Category
    {
        public Category()
        {
            Recipes = new HashSet<Recipe>();
        }

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public bool? IsDeleted { get; set; } = false;

        public ICollection<Recipe> Recipes { get; set; }
    }
}

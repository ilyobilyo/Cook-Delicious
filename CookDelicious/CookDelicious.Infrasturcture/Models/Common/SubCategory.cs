using CookDelicious.Infrasturcture.Models.Recipes;
using System.ComponentModel.DataAnnotations;

namespace CookDelicious.Infrasturcture.Models.Common
{
    public class SubCategory
    {
        public SubCategory()
        {
            Recipes = new HashSet<Recipe>();
        }

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<Recipe> Recipes { get; set; }
    }
}

using CookDelicious.Infrasturcture.Models.Recipes;
using System.ComponentModel.DataAnnotations;

namespace CookDelicious.Infrasturcture.Models.Common
{
    public class CookingTime
    {
        public CookingTime()
        {
            Recipes = new HashSet<Recipe>();
        }

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public TimeSpan Time { get; set; }

        public ICollection<Recipe> Recipes { get; set; }
    }
}

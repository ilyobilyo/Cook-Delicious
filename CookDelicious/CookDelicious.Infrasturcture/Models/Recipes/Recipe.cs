using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Infrasturcture.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookDelicious.Infrasturcture.Models.Recipes
{
    public class Recipe
    {
        public Recipe()
        {
            RecipeProducts = new HashSet<RecipeProduct>();
            Comments = new HashSet<RecipeComment>();
        }

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public ApplicationUser Author { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public DateTime PublishedOn { get; set; }

        [Required]
        public string Description { get; set; }

        public double Rating { get; set; }

        [Required]
        public Guid CookingTimeId { get; set; }

        public CookingTime CookingTime { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        public Category Catrgory { get; set; }

        public Guid SubCaregoryId { get; set; }

        [ForeignKey(nameof(SubCaregoryId))]
        public SubCategory SubCategory { get; set; }

        [Required]
        public Guid DishTypeId { get; set; }

        public DishType DishType { get; set; }

        public ICollection<RecipeProduct> RecipeProducts { get; set; }

        public ICollection<RecipeComment> Comments { get; set; }
    }
}

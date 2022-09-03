using CookDelicious.Infrasturcture.Models.Forum;
using CookDelicious.Infrasturcture.Models.Recipes;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CookDelicious.Infrasturcture.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Recipes = new HashSet<Recipe>();
            ForumPosts = new HashSet<ForumPost>();
            RecipeComments = new HashSet<RecipeComment>();
            ForumComments = new HashSet<ForumComment>();
        }

        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        public string? ImageUrl { get; set; }

        public int? Age { get; set; }

        [MaxLength(50)]
        public string? Town { get; set; }

        [MaxLength(200)]
        public string? Job { get; set; }

        [MaxLength(250)]
        public string? Address { get; set; }

        public ICollection<Recipe> Recipes { get; set; }

        public ICollection<RecipeComment> RecipeComments { get; set; }

        public ICollection<ForumPost> ForumPosts { get; set; }

        public ICollection<ForumComment> ForumComments { get; set; }
    }
}

using CookDelicious.Infrasturcture.Models.Forum;
using CookDelicious.Infrasturcture.Models.Recipes;
using Microsoft.AspNetCore.Identity;

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

        public string? ImageUrl { get; set; }

        public ICollection<Recipe> Recipes { get; set; }

        public ICollection<RecipeComment> RecipeComments { get; set; }

        public ICollection<ForumPost> ForumPosts { get; set; }

        public ICollection<ForumComment> ForumComments { get; set; }
    }
}

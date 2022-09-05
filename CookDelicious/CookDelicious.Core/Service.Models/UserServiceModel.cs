namespace CookDelicious.Core.Service.Models
{
    public class UserServiceModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string NormalizedUserName { get; set; }

        public string Email { get; set; }

        public string NormalizedEmail { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string ConcurrencyStamp { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? ImageUrl { get; set; }

        public int? Age { get; set; }

        public string? Town { get; set; }

        public string? Job { get; set; }

        public string? Address { get; set; }

        public ICollection<RecipeServiceModel> Recipes { get; set; }

        public ICollection<RecipeCommentServiceModel> RecipeComments { get; set; }

        public ICollection<ForumPostServiceModel> ForumPosts { get; set; }

        public ICollection<RecipeCommentServiceModel> ForumComments { get; set; }

    }
}

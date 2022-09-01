using CookDelicious.Infrasturcture.Models.Common;
using CookDelicious.Infrasturcture.Models.Forum;
using CookDelicious.Infrasturcture.Models.Identity;
using CookDelicious.Infrasturcture.Models.Recipes;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CookDelicious.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<DishType> DishTypes { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<RecipeProduct> RecipeProducts { get; set; }

        public DbSet<ForumComment> ForumComments { get; set; }

        public DbSet<ForumPost> ForumPosts { get; set; }

        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<RecipeComment> RecipeComments { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<PostCategory> PostCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RecipeProduct>()
                .HasKey(x => new { x.RecipeId, x.ProductId });

            builder.Entity<ForumComment>()
                .HasOne(x => x.ForumPost)
                .WithMany(x => x.ForumComments)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<RecipeComment>()
               .HasOne(x => x.Recipe)
               .WithMany(x => x.Comments)
               .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(builder);
        }
    }
}
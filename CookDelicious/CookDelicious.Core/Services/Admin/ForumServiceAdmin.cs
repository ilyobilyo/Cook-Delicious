using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Models.Admin.Forum;
using CookDelicious.Infrasturcture.Models.Forum;
using CookDelicious.Infrasturcture.Repositories;
using CookDelicious.Models;
using Microsoft.EntityFrameworkCore;

namespace CookDelicious.Core.Services.Admin
{
    public class ForumServiceAdmin : IForumServiceAdmin
    {
        private readonly IApplicationDbRepository repo;

        public ForumServiceAdmin(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task<ErrorViewModel> CreatePostCategory(CreatePostCategoryViewModel model)
        {
            if (await IsPostCategoryExists(model))
            {
                return new ErrorViewModel() { Messages = $"{model.Name} is already exist." };
            }

            var postCategory = new PostCategory()
            {
                Name = model.Name,
            };

            try
            {
                await repo.AddAsync(postCategory);
                await repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                return new ErrorViewModel() { Messages = "Unexpected error. You cant add this post category!" };
            }

            return null;
        }

        private async Task<bool> IsPostCategoryExists(CreatePostCategoryViewModel model)
        {
            return await repo.All<PostCategory>()
                .AnyAsync(x => x.Name == model.Name);
        }
    }
}

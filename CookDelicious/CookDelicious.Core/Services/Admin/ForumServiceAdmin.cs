using AutoMapper;
using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Infrasturcture.Models.Forum;
using CookDelicious.Infrasturcture.Repositories;
using CookDelicious.Models;
using Microsoft.EntityFrameworkCore;

namespace CookDelicious.Core.Services.Admin
{
    public class ForumServiceAdmin : IForumServiceAdmin
    {
        private readonly IApplicationDbRepository repo;
        private readonly IMapper mapper;

        public ForumServiceAdmin(IApplicationDbRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<ErrorViewModel> CreatePostCategory(CreatePostCategoryInputModel model)
        {
            if (await IsPostCategoryExists(model))
            {
                return new ErrorViewModel() { Messages = $"{model.Name} {MessageConstant.AlreadyExist}" };
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
                return new ErrorViewModel() { Messages = PostsConstants.UnexpectedErrorPostCategory };
            }

            return null;
        }

        public async Task DeletePost(Guid id)
        {
            var postToDelete = await repo.All<ForumPost>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            postToDelete.IsDeleted = true;

            await repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<ForumPostServiceModel>> GetAllUserPosts(string id)
        {
            var allPosts = await repo.All<ForumPost>()
                .Include(x => x.Author)
                .Where(x => x.AuthorId == id && x.IsDeleted == false)
                .ToListAsync();

            return mapper.Map<IEnumerable<ForumPostServiceModel>>(allPosts);
        }

        private async Task<bool> IsPostCategoryExists(CreatePostCategoryInputModel model)
        {
            return await repo.All<PostCategory>()
                .AnyAsync(x => x.Name == model.Name);
        }
    }
}

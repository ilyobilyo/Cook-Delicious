using AutoMapper;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Service.Models;
using CookDelicious.Infrasturcture.Models.Forum;
using CookDelicious.Infrasturcture.Models.Recipes;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CookDelicious.Core.Services.Admin
{
    public class CommentServiceAdmin : ICommentServiceAdmin
    {
        private readonly IApplicationDbRepository repo;
        private readonly IMapper mapper;


        public CommentServiceAdmin(IApplicationDbRepository repo,
            IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<bool> DeleteRecipeComment(Guid id)
        {
            var commentToDelete = await repo.All<RecipeComment>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (commentToDelete == null)
            {
                return false;
            }

            await repo.DeleteAsync<RecipeComment>(commentToDelete.Id);

            await repo.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteForumComment(Guid id)
        {
            var commentToDelete = await repo.All<ForumComment>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (commentToDelete == null)
            {
                return false;
            }

            await repo.DeleteAsync<ForumComment>(commentToDelete.Id);

            await repo.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ForumCommentServiceModel>> GetUserForumComments(string id)
        {
            var userForumComments = await repo.All<ForumComment>()
                .Include(x => x.Author)
                .Where(x => x.AuthorId == id)
                .ToListAsync();

            var forumCommentsServiceModel = mapper.Map<IEnumerable<ForumCommentServiceModel>>(userForumComments);

            return forumCommentsServiceModel;
        }

        public async Task<IEnumerable<RecipeCommentServiceModel>> GetUserRecipeComments(string userId)
        {
            var userRecipeComments = await repo.All<RecipeComment>()
                .Include(x => x.Author)
                .Where(x => x.AuthorId == userId)
                .ToListAsync();

            var recipeCommentsServiceModel = mapper.Map<IEnumerable<RecipeCommentServiceModel>>(userRecipeComments);

            return recipeCommentsServiceModel;
        }
    }
}

using AutoMapper;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Models.Admin.Comments;
using CookDelicious.Core.Models.Comments;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Core.Service.Models;
using CookDelicious.Infrasturcture.Models.Forum;
using CookDelicious.Infrasturcture.Models.Recipes;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task DeleteRecipeComment(Guid id)
        {
            var commentToDelete = await repo.All<RecipeComment>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            await repo.DeleteAsync<RecipeComment>(commentToDelete.Id);

            await repo.SaveChangesAsync();
        }

        public async Task DeleteForumComment(Guid id)
        {
            var commentToDelete = await repo.All<ForumComment>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            await repo.DeleteAsync<ForumComment>(commentToDelete.Id);

            await repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<ForumCommentServiceModel>> GetForumComments(string id)
        {
            var userForumComments = await repo.All<ForumComment>()
                .Include(x => x.Author)
                .Where(x => x.AuthorId == id)
                .ToListAsync();

            var forumCommentsServiceModel = mapper.Map<IEnumerable<ForumCommentServiceModel>>(userForumComments);

            return forumCommentsServiceModel;
        }

        public async Task<IEnumerable<RecipeCommentServiceModel>> GetRecipeComments(string userId)
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

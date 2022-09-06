using AutoMapper;
using CookDelicious.Core.Contracts.Comments;
using CookDelicious.Core.Contracts.Forum;
using CookDelicious.Core.Contracts.Recipes;
using CookDelicious.Core.Contracts.User;
using CookDelicious.Core.Models.Comments;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Infrasturcture.Models.Forum;
using CookDelicious.Infrasturcture.Models.Identity;
using CookDelicious.Infrasturcture.Models.Recipes;
using CookDelicious.Infrasturcture.Repositories;

namespace CookDelicious.Core.Services.Comments
{
    public class CommentService : ICommentService
    {
        private readonly IApplicationDbRepository repo;
        private readonly IForumService forumService;
        private readonly IUserService userService;
        private readonly IRecipeService recipeService;
        private readonly IMapper mapper;

        public CommentService(IApplicationDbRepository repo,
            IForumService forumService,
            IUserService userService,
            IRecipeService recipeService,
            IMapper mapper)
        {
            this.repo = repo;
            this.forumService = forumService;
            this.userService = userService;
            this.recipeService = recipeService;
            this.mapper = mapper;
        }

        public async Task<bool> DeletePostComment(Guid id)
        {
            bool isDeleted = false;

            var commentToDelete = await repo.GetByIdAsync<ForumComment>(id);

            if (commentToDelete == null)
            {
                return isDeleted;
            }

            commentToDelete.IsDeleted = true;

            try
            {
                await repo.SaveChangesAsync();
                isDeleted = true;
            }
            catch (Exception)
            {
                return isDeleted;
            }

            return isDeleted;
        }

        public async Task<bool> DeleteRecipeComment(Guid id)
        {
            bool isDeleted = false;

            var commentToDelete = await repo.GetByIdAsync<RecipeComment>(id);

            if (commentToDelete == null)
            {
                return isDeleted;
            }

            commentToDelete.IsDeleted = true;

            try
            {
                await repo.SaveChangesAsync();
                isDeleted = true;
            }
            catch (Exception)
            {
                return isDeleted;
            }

            return isDeleted;
        }

        public async Task<bool> PostCommentForPost(Guid id, CommentViewModel model)
        {
            var forumPost = await forumService.GetById(id);

            var user = await userService.GetUserByUsername(model.AuthorName);

            var IsPosted = false;

            if (forumPost == null || user == null)
            {
                return IsPosted;
            }

            var forumComment = new ForumComment()
            {
                //Author = user,
                AuthorId = user.Id,
                Content = model.Content,
                ForumPost = forumPost,
                ForumPostId = forumPost.Id,
                PublishedOn = DateTime.Now,
            };

            try
            {
                await repo.AddAsync(forumComment);
                await repo.SaveChangesAsync();
                IsPosted = true;
            }
            catch (Exception)
            {
                return IsPosted;
            }

            return IsPosted;
        }

        public async Task<bool> PostCommentForRecipe(Guid id, PostRecipeCommentInputModel model)
        {
            var recipePost = await recipeService.GetById(id);

            var user = await userService.GetApplicationUserByUsername(model.AuthorName);

            var IsPosted = false;

            if (recipePost == null || user == null)
            {
                return IsPosted;
            }

            var forumComment = new RecipeComment()
            {
                Author = user,
                AuthorId = user.Id,
                Content = model.Content,
                Recipe = recipePost,
                RecipeId = recipePost.Id,
                PublishedOn = DateTime.Now,
            };

            try
            {
                await repo.AddAsync(forumComment);
                await repo.SaveChangesAsync();
                IsPosted = true;
            }
            catch (Exception)
            {
                return IsPosted;
            }

            return IsPosted;
        }
    }
}

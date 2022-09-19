using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Comments;
using CookDelicious.Core.Contracts.Forum;
using CookDelicious.Core.Contracts.Recipes;
using CookDelicious.Core.Contracts.User;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Infrasturcture.Models.Forum;
using CookDelicious.Infrasturcture.Models.Recipes;
using CookDelicious.Infrasturcture.Repositories;
using CookDelicious.Models;

namespace CookDelicious.Core.Services.Comments
{
    public class CommentService : ICommentService
    {
        private readonly IApplicationDbRepository repo;
        private readonly IForumService forumService;
        private readonly IUserService userService;
        private readonly IRecipeService recipeService;

        public CommentService(IApplicationDbRepository repo,
            IForumService forumService,
            IUserService userService,
            IRecipeService recipeService)
        {
            this.repo = repo;
            this.forumService = forumService;
            this.userService = userService;
            this.recipeService = recipeService;
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
        
        public async Task<bool> PostCommentForPost(Guid id, PostCommentInputModel model)
        {
            var forumPost = await forumService.GetById(id);

            var user = await userService.GetApplicationUserByUsername(model.AuthorName);

            var IsPosted = false;

            if (forumPost == null || user == null)
            {
                return IsPosted;
            }

            var forumComment = new ForumComment()
            {
                Author = user,
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

        public async Task<ErrorViewModel> PostCommentForRecipe(Guid id, PostCommentInputModel model)
        {
            if (model.Content.Length > 200)
            {
                return new ErrorViewModel() { Messages = CommentConstants.CommentMaxLength };
            }

            var recipePost = await repo.GetByIdAsync<Recipe>(id);

            var user = await userService.GetApplicationUserByUsername(model.AuthorName);

            if (recipePost == null || user == null)
            {
                return new ErrorViewModel() { Messages = PostsConstants.InvalidRecipeOrUser };
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
            }
            catch (Exception)
            {
                return new ErrorViewModel() { Messages = MessageConstant.UnexpectedError };
            }

            return null;
        }
    }
}

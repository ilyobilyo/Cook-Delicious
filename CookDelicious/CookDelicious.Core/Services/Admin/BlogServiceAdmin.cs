using CookDelicious.Core.Constants;
using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Contracts.User;
using CookDelicious.Core.Service.Models.InputServiceModels;
using CookDelicious.Infrasturcture.Models.Blog;
using CookDelicious.Infrasturcture.Repositories;
using CookDelicious.Models;
using Microsoft.EntityFrameworkCore;

namespace CookDelicious.Core.Services.Admin
{
    public class BlogServiceAdmin : IBlogServiceAdmin
    {
        private readonly IApplicationDbRepository repo;
        private readonly IUserService userService;

        public BlogServiceAdmin(IApplicationDbRepository repo, IUserService userService)
        {
            this.repo = repo;
            this.userService = userService;
        }

        public async Task<ErrorViewModel> CreateBlogPost(CreateBlogPostInputModel inputModel, string username)
        {
            if (inputModel.Title == null || inputModel.Description == null)
            {
                return new ErrorViewModel() { Messages = PostsConstants.RequiredTitleAndContent };
            }

            var author = await userService.GetApplicationUserByUsername(username);

            if (author == null)
            {
                return new ErrorViewModel() { Messages = UserConstants.InvalidAuthor };
            }

            var blogPostCategory = await GetBlogPostCategoryByName(inputModel.Category);

            var blogPost = new BlogPost()
            {
                Author = author,
                AuthorId = author.Id,
                Content = inputModel.Description,
                ImageUrl = inputModel.ImageUrl,
                BlogPostCategory = blogPostCategory,
                Title = inputModel.Title,
                PublishedOn = DateTime.Now,
            };

            try
            {
                await repo.AddAsync(blogPost);
                await repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                return new ErrorViewModel() { Messages = PostsConstants.UnexpectedErrorPost };
            }

            return null;
        }

        

        public async Task<ErrorViewModel> CreateBlogPostCategory(CreateBlogPostCategoryInputModel inputModel)
        {
            if (await IsBlogPostCategoryExists(inputModel))
            {
                return new ErrorViewModel() { Messages = $"{inputModel.Name} {MessageConstant.AlreadyExist}" };
            }

            var blogPostCategory = new BlogPostCategory()
            {
                Name = inputModel.Name,
            };

            try
            {
                await repo.AddAsync(blogPostCategory);
                await repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                return new ErrorViewModel() { Messages = PostsConstants.UnexpectedErrorPostCategory };
            }

            return null;
        }

        public async Task<bool> DeleteBlogPost(Guid id)
        {
            bool isDeleted = false;

            var postToDelete = await repo.GetByIdAsync<BlogPost>(id);

            if (postToDelete == null)
            {
                return isDeleted;
            }

            postToDelete.IsDeleted = true;

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

        public async Task<IList<string>> GetAllBlogPostCategoryNames()
        {
            return await repo.All<BlogPostCategory>()
                .Select(x => x.Name)
                .ToListAsync();
        }



        private async Task<BlogPostCategory> GetBlogPostCategoryByName(string category)
        {
            return await repo.All<BlogPostCategory>()
                .Where(x => x.Name == category)
                .FirstOrDefaultAsync();
        }

        private async Task<bool> IsBlogPostCategoryExists(CreateBlogPostCategoryInputModel model)
        {
            return await repo.All<BlogPostCategory>()
                .AnyAsync(x => x.Name == model.Name);

        }
    }
}

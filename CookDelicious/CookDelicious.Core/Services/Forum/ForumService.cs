using CookDelicious.Core.Contracts.Comments;
using CookDelicious.Core.Contracts.Forum;
using CookDelicious.Core.Contracts.User;
using CookDelicious.Core.Models.Comments;
using CookDelicious.Core.Models.Forum;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Infrasturcture.Models.Forum;
using CookDelicious.Infrasturcture.Repositories;
using CookDelicious.Models;
using Microsoft.EntityFrameworkCore;

namespace CookDelicious.Core.Services.Forum
{
    public class ForumService : IForumService
    {
        private readonly IApplicationDbRepository repo;
        private readonly IUserService userService;
        public ForumService(IApplicationDbRepository repo, IUserService userService)
        {
            this.repo = repo;
            this.userService = userService;
        }

        public async Task<ForumPost> GetById(Guid id)
        {
            return await repo.GetByIdAsync<ForumPost>(id);
        }

        public async Task<ErrorViewModel> CreatePost(CreatePostViewModel model, string Username)
        {
            if (model.Title == null || model.Description == null)
            {
                return new ErrorViewModel() { Messages = "Заглавието и садържанието на поста са задължителни!"};
            }

            var author = await userService.GetUserByUsername(Username);

            if (author == null)
            {
                return new ErrorViewModel() { Messages = "Невалиден автор. Пробвайте пак!" };
            }

            var postCategory = await GetPostCategoryByName(model.Category);

            var forumPost = new ForumPost()
            {
                Author = author,
                AuthorId = author.Id,
                Content = model.Description,
                ImageUrl = model.ImageUrl,
                PostCategory = postCategory,
                Title = model.Title,
                PublishedOn = DateTime.Now,
            };

            try
            {
                await repo.AddAsync(forumPost);
                await repo.SaveChangesAsync();
            }
            catch (Exception)
            {
                return new ErrorViewModel() { Messages = "Unexpected error. You cant add this post!" };
            }

            return null;
        }

        public async Task<bool> DeletePost(Guid id)
        {
            bool isDeleted = false;

            var postToDelete = await repo.GetByIdAsync<ForumPost>(id);

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

        public async Task<IList<string>> GetAllPostCategoryNames()
        {
            return await repo.All<PostCategory>()
                .Select(x => x.Name)
                .ToListAsync();
        }

        public async Task<PagingList<ForumPostViewModel>> GetAllPosts(int pageNumber)
        {
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            int pageSize = 6;

            return await PagingList<ForumPostViewModel>.CreateAsync(repo.All<ForumPost>()
                .Where(x => x.IsDeleted == false)
                .Select(x => new ForumPostViewModel
                {
                    Id = x.Id,
                    AuthorName = x.Author.UserName,
                    CategoryName = x.PostCategory.Name,
                    Content = x.Content,
                    DatePublishedOn = x.PublishedOn.ToString("dd"),
                    MonthPublishedOn = x.PublishedOn.ToString("MMMM"),
                    YearPublishedOn = x.PublishedOn.ToString("yyyy"),
                    ImageUrl = x.ImageUrl,
                    Title = x.Title,
                }),
                pageNumber,
                pageSize);
        }

        public async Task<IList<string>> GetArchive()
        {
            var posts = await repo.All<ForumPost>()
                .ToListAsync();

            return posts
                .DistinctBy(x => x.PublishedOn.Month)
                .Select(x => x.PublishedOn.ToString("MMMM yyyy"))
                .Take(5)
                .ToList();
        }

        public async Task<ForumPostViewModel> GetPostById(Guid id, int commentPage)
        {
            if (commentPage == 0)
            {
                commentPage = 1;
            }

            int pageSize = 5;

            var post = await repo.All<ForumPost>()
                .Where(x => x.Id == id)
                .Select(x => new ForumPostViewModel()
                {
                    Id = x.Id,
                    AuthorName = x.Author.UserName,
                    CategoryName = x.PostCategory.Name,
                    Content = x.Content,
                    DatePublishedOn = x.PublishedOn.ToString("dd"),
                    MonthPublishedOn = x.PublishedOn.ToString("MMMM"),
                    YearPublishedOn = x.PublishedOn.ToString("yyyy"),
                    ImageUrl = x.ImageUrl,
                    Title = x.Title,
                })
                .FirstOrDefaultAsync();


            post.Comments = await PagingList<CommentViewModel>.CreateAsync(repo.All<ForumComment>()
                .Where(x => x.IsDeleted == false && x.ForumPostId == post.Id)
                .Select(x => new CommentViewModel
                {
                    Id = x.Id,
                    AuthorName = x.Author.UserName,
                    Content = x.Content,
                }),
                commentPage,
                pageSize);

            return post;

        }





        private async Task<PostCategory> GetPostCategoryByName(string category)
        {
            return await repo.All<PostCategory>()
                .Where(x => x.Name == category)
                .FirstOrDefaultAsync();
        }
    }
}

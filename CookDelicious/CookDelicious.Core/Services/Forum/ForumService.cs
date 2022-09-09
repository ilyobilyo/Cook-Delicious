using AutoMapper;
using CookDelicious.Core.Contracts.Comments;
using CookDelicious.Core.Contracts.Forum;
using CookDelicious.Core.Contracts.User;
using CookDelicious.Core.Models.Comments;
using CookDelicious.Core.Models.Forum;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Core.Service.Models;
using CookDelicious.Core.Service.Models.InputServiceModels;
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
        private readonly IMapper mapper;

        public ForumService(IApplicationDbRepository repo, 
            IUserService userService,
            IMapper mapper)
        {
            this.repo = repo;
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task<ForumPost> GetById(Guid id)
        {
            return await repo.GetByIdAsync<ForumPost>(id);
        }

        public async Task<ErrorViewModel> CreatePost(CreateForumPostInputModel model, string Username)
        {
            if (model.Title == null || model.Description == null)
            {
                return new ErrorViewModel() { Messages = "Заглавието и садържанието на поста са задължителни!"};
            }

            var author = await userService.GetApplicationUserByUsername(Username);

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

        public async Task<(IEnumerable<ForumPostServiceModel>, int)> GetAllPostsForPageing(int pageNumber, int pageSize)
        {
            var totalPostsCount = await repo.All<ForumPost>()
                .CountAsync();

            var posts = await repo.All<ForumPost>()
                .Include(x => x.Author)
                .Include(x => x.PostCategory)
                .Where(x => x.IsDeleted == false)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var forumPostServiceModels = mapper.Map<IEnumerable<ForumPostServiceModel>>(posts);

            return (forumPostServiceModels, totalPostsCount);
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

        public async Task<ForumPostServiceModel> GetPostServiceModelById(Guid id)
        {
            var post = await repo.All<ForumPost>()
                .Include(x => x.Author)
                .Include(x => x.PostCategory)
                .Include(x => x.ForumComments.Where(c => c.IsDeleted == false))
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            var postServiceModel = mapper.Map<ForumPostServiceModel>(post);

            return postServiceModel;
        }


        public async Task<IEnumerable<ForumCommentServiceModel>> GetCommentsPerPage(Guid id, int commentPage, int pageSize)
        {
            var forumComments = await repo.All<ForumComment>()
                .Include(x => x.Author)
                .Where(x => x.ForumPostId == id && x.IsDeleted == false)
                .Skip((commentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var forumCommentsServiceModel = mapper.Map<IEnumerable<ForumCommentServiceModel>>(forumComments);

            return forumCommentsServiceModel;
        }



        private async Task<PostCategory> GetPostCategoryByName(string category)
        {
            return await repo.All<PostCategory>()
                .Where(x => x.Name == category)
                .FirstOrDefaultAsync();
        }

    }
}

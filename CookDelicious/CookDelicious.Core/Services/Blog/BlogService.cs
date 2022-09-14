using AutoMapper;
using CookDelicious.Core.Contracts.Blog;
using CookDelicious.Core.Service.Models;
using CookDelicious.Infrasturcture.Models.Blog;
using CookDelicious.Infrasturcture.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CookDelicious.Core.Services.BlogService
{
    public class BlogService : IBlogService
    {
        private readonly IApplicationDbRepository repo;
        private readonly IMapper mapper;

        public BlogService(IApplicationDbRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<(IEnumerable<BlogPostServiceModel>, int)> GetAllSortBlogPostsForPageing(int pageNumber, int pageSize, string blogPostCategory, int? sortMonth)
        {
            var totalBlogPostsCount = 0;

            var blogPosts = new List<BlogPost>();

            if (blogPostCategory == null && sortMonth == null)
            {
                totalBlogPostsCount = await repo.All<BlogPost>()
                .Where(x => x.IsDeleted == false)
                .CountAsync();

                blogPosts = await repo.All<BlogPost>()
                   .Include(x => x.Author)
                   .Include(x => x.BlogPostCategory)
                   .Where(x => x.IsDeleted == false)
                   .Skip((pageNumber - 1) * pageSize)
                   .Take(pageSize)
                   .ToListAsync();
            }
            else
            {
                if (sortMonth == null)
                {
                    totalBlogPostsCount = await repo.All<BlogPost>()
                    .Include(x => x.BlogPostCategory)
                    .Where(x => x.IsDeleted == false && x.BlogPostCategory.Name == blogPostCategory)
                    .CountAsync();

                    blogPosts = await repo.All<BlogPost>()
                      .Include(x => x.Author)
                      .Include(x => x.BlogPostCategory)
                      .Where(x => x.IsDeleted == false && x.BlogPostCategory.Name == blogPostCategory)
                      .Skip((pageNumber - 1) * pageSize)
                      .Take(pageSize)
                      .ToListAsync();
                }
                else
                {
                    totalBlogPostsCount = await repo.All<BlogPost>()
                    .Where(x => x.IsDeleted == false && x.PublishedOn.Month == sortMonth)
                    .CountAsync();

                    blogPosts = await repo.All<BlogPost>()
                      .Include(x => x.Author)
                      .Include(x => x.BlogPostCategory)
                      .Where(x => x.IsDeleted == false && x.PublishedOn.Month == sortMonth)
                      .Skip((pageNumber - 1) * pageSize)
                      .Take(pageSize)
                      .ToListAsync();
                }
                
            }

            var blogPostServiceModels = mapper.Map<IEnumerable<BlogPostServiceModel>>(blogPosts);

            return (blogPostServiceModels, totalBlogPostsCount);
        }

        public async Task<IList<string>> GetBlogAllPostCategoryNames()
        {
            return await repo.All<BlogPostCategory>()
                .Select(x => x.Name)
                .ToListAsync();
        }

        public async Task<IList<string>> GetBlogArchive()
        {
            var blogPosts = await repo.All<BlogPost>()
                .ToListAsync();

            return blogPosts
                .DistinctBy(x => x.PublishedOn.Month)
                .Select(x => x.PublishedOn.ToString("MM yyyy"))
                .Take(5)
                .ToList();
        }

        public async Task<BlogPostServiceModel> GetBlogPostServiceModelById(Guid id)
        {
            var blogPost = await repo.All<BlogPost>()
                .Include(x => x.Author)
                .Include(x => x.BlogPostCategory)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            var blogPostServiceModel = mapper.Map<BlogPostServiceModel>(blogPost);

            return blogPostServiceModel;
        }
    }
}

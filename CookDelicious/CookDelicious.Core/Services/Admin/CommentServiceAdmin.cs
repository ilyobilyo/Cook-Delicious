using CookDelicious.Core.Contracts.Admin;
using CookDelicious.Core.Models.Admin.Comments;
using CookDelicious.Core.Models.Comments;
using CookDelicious.Core.Models.Paiging;
using CookDelicious.Infrasturcture.Models.Forum;
using CookDelicious.Infrasturcture.Models.Recipes;
using CookDelicious.Infrasturcture.Repositories;
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

        public CommentServiceAdmin(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task<PagingList<AdminCommentViewModel>> GetAllComments(int pageNumber)
        {
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            int pageSize = 10;

            var list = await PagingList<AdminCommentViewModel>.CreateAsync(repo.All<ForumComment>()
                .Select(x => new AdminCommentViewModel()
                {
                    Id = x.Id,
                    AuthorName = x.Author.UserName,
                    Content = x.Content,
                    IsDeleted = x.IsDeleted,
                }),
                pageNumber,
                pageSize);

            list.AddRange(repo.All<RecipeComment>()
                .Select(x => new AdminCommentViewModel()
                {
                    AuthorName = x.Author.UserName,
                    Id = x.Id,
                    Content = x.Content,
                    IsDeleted = x.IsDeleted
                }));

            return list;
        }
    }
}

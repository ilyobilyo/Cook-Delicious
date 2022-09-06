using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookDelicious.Core.Service.Models.InputServiceModels
{
    public class PostRecipeCommentInputModel
    {
        public Guid Id { get; set; }

        public string AuthorName { get; set; }

        public string Content { get; set; }
    }
}

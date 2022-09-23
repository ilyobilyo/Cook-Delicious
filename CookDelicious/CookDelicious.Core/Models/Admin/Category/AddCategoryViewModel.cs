using CookDelicious.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace CookDelicious.Core.Models.Admin.Category
{
    public class AddCategoryViewModel
    {
        [Required(ErrorMessage = MessageConstant.RequiredName)]
        public string Name { get; set; }
    }
}

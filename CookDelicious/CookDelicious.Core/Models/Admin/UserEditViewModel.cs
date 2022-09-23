using CookDelicious.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace CookDelicious.Core.Models.Admin
{
    public class UserEditViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = UserConstants.UsernameRequired)]
        public string Username { get; set; }
    }
}

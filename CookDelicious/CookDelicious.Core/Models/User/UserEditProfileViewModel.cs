using CookDelicious.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace CookDelicious.Core.Models.User
{
    public class UserEditProfileViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = UserConstants.UsernameRequired)]
        public string Username { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [Required(ErrorMessage =UserConstants.EmailRequired)]
        public string Email { get; set; }

        public int? Age { get; set; }

        public string Town { get; set; }

        public string ImageUrl { get; set; }

        public string? Job { get; set; }

        public string? Address { get; set; }

    }
}

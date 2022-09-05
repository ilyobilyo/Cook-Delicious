namespace CookDelicious.Core.Service.Models.InputServiceModels
{
    public class UserEditProfileInputModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Email { get; set; }

        public int? Age { get; set; }

        public string Town { get; set; }

        public string ImageUrl { get; set; }

        public string? Job { get; set; }

        public string? Address { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookDelicious.Core.Models.User
{
    public class UserProfileViewModel
    {
        public string Username { get; set; }

        public int Age { get; set; }

        public string Town { get; set; }

        public string ImageUrl { get; set; }

        public List<MyRecipesViewModel> MyRecipes { get; set; }
    }
}

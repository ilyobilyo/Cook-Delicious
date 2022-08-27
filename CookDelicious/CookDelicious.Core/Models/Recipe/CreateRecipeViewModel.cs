using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookDelicious.Core.Models.Recipe
{
    public class CreateRecipeViewModel
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public string CookingTime { get; set; }

        public string Category { get; set; }

        public string SubCategory { get; set; }

        public string DishType { get; set; }

        public string Products { get; set; }
    }
}

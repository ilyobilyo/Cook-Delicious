using System.ComponentModel.DataAnnotations;

namespace CookDelicious.Core.Service.Models.InputServiceModels
{
    public class CreateProductInputModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Type { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }
    }
}

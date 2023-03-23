using System.ComponentModel.DataAnnotations;

namespace Forms.ViewModel
{
    public class CreateProductViewModel
    {
        [Required]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Product Description")]
        public string Description { get; set; }

        [Display(Name = "Product Price")]
        public float Price { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Image")]
        //to apply file type for image
        public IFormFile ImageUrl { get; set; }
        //public string ImageUrl { get; set; }
    }
}

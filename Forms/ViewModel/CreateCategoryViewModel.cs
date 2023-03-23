using System.ComponentModel.DataAnnotations;

namespace Forms.ViewModel
{
    public class CreateCategoryViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}

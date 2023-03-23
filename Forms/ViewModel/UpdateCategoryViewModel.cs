using System.ComponentModel.DataAnnotations;

namespace Forms.ViewModel
{
    public class UpdateCategoryViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}

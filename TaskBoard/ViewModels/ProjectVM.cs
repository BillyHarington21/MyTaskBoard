using System.ComponentModel.DataAnnotations;

namespace TaskBoard.ViewModels
{
    public class ProjectVM
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = "";

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = "";

    }
}

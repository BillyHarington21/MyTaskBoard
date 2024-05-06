using System.ComponentModel.DataAnnotations;

namespace TaskBoard.Models
{
    public class Project
    {      
        
        [Key]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Sprint> Sprints { get; set; }        
    }
}

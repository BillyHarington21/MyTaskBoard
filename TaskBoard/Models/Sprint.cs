using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TaskBoard.Models
{
    public class Sprint
    {
   
        public int Id { get; set; }
        public string Name { get; set; }    
        public string Description { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ProjectName  { get; set; }
        public Project Project { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public ICollection<TaskWork> Tasks { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using TaskBoard.Models;

namespace TaskBoard.ViewModels
{
    public class SprintVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ProjectName { get; set; }
        public string? UserId {  get; set; }  
        
       
    }
}

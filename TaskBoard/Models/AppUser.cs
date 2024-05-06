using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TaskBoard.Models
{
    public class AppUser : IdentityUser
    {
      
        [Required]
        public string? Name {  get; set; }
        public ICollection<Sprint> Sprints { get; set; }
        public UserRole Role { get; set; }
        public bool IsBlocked { get; set; }
        public enum UserRole
        {
            Administrator,
            Manager,
            User
        }
        
    }
}

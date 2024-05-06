using System.ComponentModel.DataAnnotations;

namespace TaskBoard.ViewModels
{
    public class AppUserVM
    {
        
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool IsBlocked { get; set; }
        public UserRole Role { get; set; }       
        public enum UserRole 
        {
            Administrator,
            Manager,
            User
        }
    }
}

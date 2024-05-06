using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using TaskBoard.Data;
using TaskBoard.Interfaces;
using TaskBoard.Models;
using TaskBoard.ViewModels;


namespace TaskBoard.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _appDbContext ;

        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
       

        public IActionResult Index()
        {       

            return View();
        }
       
        public IActionResult Privacy()
        {
            // Получаем имя текущего пользователя
            var userName = User.Identity.Name;

            // Извлекаем пользователя из базы данных по его имени
            var user = _appDbContext.AppUsers.FirstOrDefault(u => u.UserName == userName);

            // Проверяем, что пользователь найден
            if (user != null)
            {
                // Получаем роль пользователя и сохраняем ее в ViewBag
                ViewBag.UserRole = user.Role.ToString();
            }
            return View();
        }

        public IActionResult ManagerOfUsers()
        {
            var users = _appDbContext.AppUsers.Select(p => new AppUserVM { Name = p.Name, Email = p.Email, Role = (AppUserVM.UserRole)p.Role, IsBlocked = p.IsBlocked }).ToList();
            return View(users);
        }
        public IActionResult AddRole(string UserName, AppUserVM.UserRole role)
        {
            var user = _appDbContext.AppUsers.FirstOrDefault(q => q.Name == UserName);
            if (user == null)
            {
                return NotFound();
            }
            user.Role = (AppUser.UserRole)role;
            _appDbContext.SaveChanges();
            return RedirectToAction("ManagerOfUsers");
        }
        [HttpPost]
        public IActionResult ToggleBlockUser(string UserName, bool isBlocked)
        {
            var user = _appDbContext.AppUsers.FirstOrDefault(q => q.Name == UserName);
            if (user == null)
            {
                return NotFound();
            }

            user.IsBlocked = isBlocked;  // Переключаем статус блокировки

            _appDbContext.SaveChanges();

            return RedirectToAction("ManagerOfUsers");
        }
    }
}

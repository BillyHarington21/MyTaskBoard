using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Data;
using TaskBoard.Models;
using TaskBoard.ViewModels;

namespace TaskBoard.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;
        private readonly AppDbContext _appDbContext; 
        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, AppDbContext appDb )
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            _appDbContext = appDb;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid) 
            {
                var result = await signInManager.PasswordSignInAsync(model.Username!,model.Password!,false,false);
                var Name = model.Username;
                if (result.Succeeded)
                {
                    var user = _appDbContext.AppUsers.FirstOrDefault(u => u.UserName == Name);
                    if (user.IsBlocked == false)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        await signInManager.SignOutAsync();
                        ModelState.AddModelError("", "Your account is blocked. Please contact support for assistance.");
                        return View(model);
                    }
                }
                ModelState.AddModelError("","Error");
                return View(model);
            }
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser()
                {
                    Name = model.Name,
                    UserName = model.Email,
                    Email = model.Email,
                };

                var result = await userManager.CreateAsync(user, model.Password!);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user,false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors) 
                {
                   ModelState.AddModelError("",error.Description);
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}

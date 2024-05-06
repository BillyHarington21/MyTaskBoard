using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskBoard.Data;
using TaskBoard.Models;

namespace TaskBoard.RoleRequired
{
    public class RoleAuthorizationAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly int _requiredRole;

        public RoleAuthorizationAttribute(int requiredRole)
        {
            _requiredRole = requiredRole;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>(); // Замените YourDbContext на ваш контекст данных
            var userName = context.HttpContext.User.Identity.Name;

            var user = dbContext.AppUsers.FirstOrDefault(u => u.Name == userName);

            if (user == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            
            // Проверяем роль пользователя
            if (user.Role != (AppUser.UserRole)_requiredRole)
            {
                context.Result = new ForbidResult();
                return;
            }

            // Разрешаем доступ
            return;
        }
    }
}


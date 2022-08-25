using Microsoft.AspNetCore.Identity;
using Repository.Models;

namespace Application.Authorization
{
    public interface IIdentityManager
    {
        void CreateUser(User user);
        void CreateRoles(RoleManager<IdentityRole>? roleManager);
        ApplicationUser GetUser(string? userId);
    }
}
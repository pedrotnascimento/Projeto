﻿using Microsoft.AspNetCore.Identity;
using Repository;
using Repository.Models;
using System.Security.Cryptography.Xml;

namespace Application.Authorization
{
    public class IdentityManager : IIdentityManager
    {
        private readonly AppDatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public IdentityManager(
            AppDatabaseContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        
        public async void CreateUser(
            User user)
        {
            var userApp = new ApplicationUser()
            {
                Email = user.Email,
                EmailConfirmed = true,
                UserName = user.Name
            };


            var result = await _userManager.FindByNameAsync(user.Name); 
            if (result==null)
            {
                var resultado = await _userManager
                    .CreateAsync(userApp, user.Password);

                if (resultado.Succeeded &&
                    !string.IsNullOrWhiteSpace(user.Role))
                {
                    await _userManager.AddToRoleAsync(userApp, user.Role);
                }
                else
                {
                    throw new Exception("Users password invalid!");
                }
            }
            else
            {
                throw new Exception("User already exists!");
            }
        }

        public void CreateRoles(RoleManager<IdentityRole>? roleManager)
        {
            if (!_roleManager.RoleExistsAsync(Roles.ROLE_CLIENT).Result)
            {
                var resultado = roleManager.CreateAsync(
                    new IdentityRole(Roles.ROLE_CLIENT)).Result;
                if (!resultado.Succeeded)
                {
                    throw new Exception(
                        $"Error during role {Roles.ROLE_CLIENT} creation.");
                }
            }
        }

        public ApplicationUser GetUser(string? userId)
        {
            return _userManager.Users.Where(x => x.Id == userId).FirstOrDefault();
        }
    }
}

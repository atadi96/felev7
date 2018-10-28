using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Portal.MVC.Models;
using Portal.Persistence;

namespace Portal.MVC.Services
{
    public class BuyerService
    {
        private readonly UserManager<DbUser> userManager;
        private readonly SignInManager<DbUser> signInManager;
        private readonly RoleManager<DbUser> roleManager;
        private readonly HttpContext httpContext;

        public BuyerService(
            IHttpContextAccessor ctxAcc,
            UserManager<DbUser> userManager,
            SignInManager<DbUser> signInManager
            //RoleManager<DbUser> roleManager
        ) {
            this.httpContext = ctxAcc.HttpContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
            //this.roleManager = roleManager;
        }

        public DbUser CurrentDbUser() {
            DbUser user = userManager.GetUserAsync(httpContext.User).Result;
            return user;
        }

        public IdentityResult Register(BuyerViewModel vm)
        {
            DbUser newUser = new DbUser
            {
                Name = vm.Name,
                UserName = vm.UserName,
                Email = vm.Email,
                PhoneNumber = vm.PhoneNumber
            };

            var result = userManager.CreateAsync(newUser, vm.Password).Result;
            if(result.Succeeded)
            {
                var roleResult = userManager.AddToRoleAsync(newUser, "buyer").Result;
                if (roleResult.Succeeded)
                {
                    signInManager.SignInAsync(newUser, isPersistent: false);
                }
                else
                {
                    return roleResult;
                }
            }
            return result;
        }

        public SignInResult SignIn(string userName, string password)
        {
            var result = signInManager.PasswordSignInAsync(userName, password, false, false).Result;
            return result;
        }

        public void Logout(int id)
        {
            try
            {
                signInManager.SignOutAsync();
            }
            catch
            {
                
                throw;
            }
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly HttpContext httpContext;

        public BuyerService(
            IHttpContextAccessor ctxAcc,
            UserManager<DbUser> userManager,
            SignInManager<DbUser> signInManager
            //RoleManager<IdentityRole> roleManager
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

        public async Task<IdentityResult> Register(BuyerViewModel vm)
        {
            DbUser newUser = new DbUser
            {
                Name = vm.Name,
                UserName = vm.UserName,
                Email = vm.Email,
                PhoneNumber = vm.PhoneNumber
            };

            var result = await userManager.CreateAsync(newUser, vm.Password);
            if(result.Succeeded)
            {
                //var roleResult = await userManager.AddToRoleAsync(newUser, "buyer");
                //if (roleResult.Succeeded)
                //{
                    await signInManager.SignInAsync(newUser, isPersistent: false);
                //}
                //else
                //{
                //    return roleResult;
                //}
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
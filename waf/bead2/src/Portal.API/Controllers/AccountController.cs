using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Portal.Persistence;
using Portal.Persistence.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Portal.API.Controllers
{
    [Produces("application/json")]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private SignInManager<DbUser> signInManager;
        private UserManager<DbUser> userManager;
        
        public AccountController(SignInManager<DbUser> signInManager, UserManager<DbUser> userManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        // POST: api/account/login/username/password
        [HttpGet("login/{username}/{password}")]
        [Produces("application/json")]
        public async Task<PublisherDTO> Login(string username, string password)
        {
            if (signInManager.IsSignedIn(User))
            {
                string userName = User.Identity.Name;
                var user = await userManager.FindByNameAsync(userName);
                return new PublisherDTO()
                {
                    Name = user.Name
                };
            }
            else
            {
                var result = await signInManager.PasswordSignInAsync(username, password, true, false);
                if (result.Succeeded)
                {
                    var user = await userManager.FindByNameAsync(username);
                    return new PublisherDTO()
                    {
                        Name = user.Name
                    };
                }
                else
                {
                    return null;
                }
            }
        }
        
        // DELETE: api/account/logout
        [HttpGet("logout")]
        [Authorize(Roles = "Publisher")]
        public async void Logout()
        {
            await signInManager.SignOutAsync();
        }
    }
}
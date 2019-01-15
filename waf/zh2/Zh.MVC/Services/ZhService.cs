using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Zh.MVC.Models;
using Zh.Persistence;

namespace Zh.MVC.Services
{
    public class ZhService
    {
        private readonly UserManager<DbUser> userManager;
        private readonly SignInManager<DbUser> signInManager;
        private readonly ZhContext context;
        private readonly HttpContext httpContext;

        public ZhService(
            IHttpContextAccessor ctxAcc,
            ZhContext context,
            UserManager<DbUser> userManager,
            SignInManager<DbUser> signInManager
        ) {
            this.httpContext = ctxAcc.HttpContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        public async Task<DbUser> CurrentDbUser() {
            DbUser user = await userManager.GetUserAsync(httpContext.User);
            return user;
        }

        public IQueryable<Item> Items()
        {
            var query =
                context.Items
                    .Include(item => item.Things)
                    .Include(item => item.DbUser);
            return query;
        }

        public ItemViewModel GetItemForId(int id)
        {
            return
                context.Items
                    .Include(item => item.Things)
                    .Include(item => item.DbUser)
                    .Where(item => item.Id == id)
                    .Select(item => new ItemViewModel() {
                        //TODO
                    })
                    .FirstOrDefault();
        }
    }
}
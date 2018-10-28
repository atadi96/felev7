using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Portal.MVC.Models;
using Portal.Persistence;

namespace Portal.MVC.Services
{
    public class PortalService
    {
        private readonly UserManager<DbUser> userManager;
        private readonly SignInManager<DbUser> signInManager;
        private readonly PortalContext portalContext;
        private readonly HttpContext httpContext;

        public PortalService(
            IHttpContextAccessor ctxAcc,
            PortalContext portalContext,
            UserManager<DbUser> userManager,
            SignInManager<DbUser> signInManager
        ) {
            this.httpContext = ctxAcc.HttpContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.portalContext = portalContext;
        }

        public IQueryable<ItemPreviewViewModel> HomeItems(string nameSearch, string categorySearch) =>
            portalContext.Items
                .Include(item => item.Publisher)
                .Include(item => item.Bids)
                .Include(item => item.Category)
                .Where(item =>
                    (string.IsNullOrWhiteSpace(nameSearch) || item.Name.ToLower().Contains(nameSearch.ToLower())) &&
                    (string.IsNullOrWhiteSpace(categorySearch) || item.Category.Name == categorySearch)
                ).OrderByDescending(item => item.Id)
                .Select(item =>
                    new ItemPreviewViewModel(
                        item,
                        item.Bids
                            .Select(bid => bid.Amout)
                            .DefaultIfEmpty(item.InitLicit)
                            .Max()
                    )
                );
        public IEnumerable<SelectListItem> CategorySelectList() =>
            portalContext.Categories
                .Select(category => new SelectListItem(category.Name, category.Name))
                .ToArray();

        public async Task<DbUser> CurrentDbUser() {
            DbUser user = await userManager.GetUserAsync(httpContext.User);
            return user;
        }

        public void SetViewDataUser(ViewDataDictionary viewData)
        {
            viewData["CurrentUser"] = CurrentDbUser().Result;
        }

        public ItemViewModel GetItemForId(int id) =>
            portalContext.Items
                .Include(item => item.Bids)
                .Include(item => item.Publisher)
                .Include(item => item.Category)
                .Where(item => item.Id == id)
                .Select(item => new ItemViewModel
                    {
                        Category = item.Category.Name,
                        Description = item.Description,
                        Expiration = item.Expiration,
                        Publisher = item.Publisher.Name,
                        Name = item.Name,
                        Image = item.Image,
                        PublishDate = item.PublishDate,
                        CurrentLicit =
                            item.Bids
                                .Select(bid => bid.Amout)
                                .DefaultIfEmpty(item.InitLicit)
                                .Max()
                    }
                )
                .FirstOrDefault();
    }
}
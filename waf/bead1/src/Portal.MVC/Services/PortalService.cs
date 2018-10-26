using System.Linq;
using Microsoft.AspNetCore.Identity;
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

        public PortalService(PortalContext portalContext, UserManager<DbUser> userManager, SignInManager<DbUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.portalContext = portalContext;
        }

        public IQueryable<ItemPreviewViewModel> HomeItems(string nameSearch, string categorySearch)
        {
            return portalContext.Items
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
                        item.Bids.Select(bid => bid.Amout).Max()
                    )
                );
        }
    }
}
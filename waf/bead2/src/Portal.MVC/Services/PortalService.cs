using System;
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
                            .Select(bid => bid.Amount)
                            .DefaultIfEmpty(item.InitLicit)
                            .Max()
                    )
                );

        public IEnumerable<ItemPreviewViewModel> GetItemsInCategory(string id)
        {
            return portalContext.Items
                .Include(item => item.Publisher)
                .Include(item => item.Category)
                .Where(item => item.Category.Name == id && item.Expiration > DateTime.Now)
                .OrderByDescending(item => item.PublishDate)
                .Select(item => new ItemPreviewViewModel(
                    item,
                    item.Bids
                        .Select(bid => bid.Amount)
                        .DefaultIfEmpty(item.InitLicit)
                        .Max()
                ));
        }

        public IEnumerable<SelectListItem> CategorySelectList() =>
            portalContext.Categories
                .Select(category => new SelectListItem(category.Name, category.Name))
                .AsEnumerable()
                .ToArray();

        public async Task<DbUser> CurrentDbUser() {
            DbUser user = await userManager.GetUserAsync(httpContext.User);
            return user;
        }

        public void SetViewDataUser(ViewDataDictionary viewData)
        {
            viewData["CurrentUser"] = CurrentDbUser().Result?.UserName;
        }

        public IEnumerable<MyItemsViewModel> MyItems()
        {
            DbUser currentUser = CurrentDbUser().Result ?? throw new System.Exception("A user must be logged in");
            return portalContext.Items
                .Include(item => item.Bids)
                .ThenInclude(bid => bid.User)
                .Include(item => item.Publisher)
                .Where(item => item.Bids.Where(bid => bid.User == currentUser).Any())
                .Select(item => new MyItemsViewModel
                {
                    Active = item.Expiration > System.DateTime.Now,
                    Item = new ItemPreviewViewModel(
                        item,
                        item.Bids
                            .Select(bid => bid.Amount)
                            .DefaultIfEmpty(item.InitLicit)
                            .Max()
                    ),
                    OwnBid = item.Bids
                        .Where(bid => bid.User == currentUser)
                        .OrderByDescending(bid => bid.Amount)
                        .Select(bid => bid.Amount)
                        .First()
                }
                );
        }

        public ItemViewModel GetItemForId(int id) =>
            portalContext.Items
                .Include(item => item.Bids)
                .Include(item => item.Publisher)
                .Include(item => item.Category)
                .Where(item => item.Id == id)
                .Select(item => new ItemViewModel
                    {
                        Id = item.Id,
                        Category = item.Category.Name,
                        Description = item.Description,
                        Expiration = item.Expiration,
                        Publisher = item.Publisher.Name,
                        Name = item.Name,
                        Image = item.Image,
                        PublishDate = item.PublishDate,
                        InitLicit = item.InitLicit,
                        CurrentLicit =
                            item.Bids
                                .OrderByDescending(bid => bid.Amount)
                                .Select(bid => (int?)bid.Amount)
                                .FirstOrDefault()
                    }
                )
                .FirstOrDefault();

        public bool PlaceBidint(int itemId, int amount)
        {
            DbUser currentUser = CurrentDbUser().Result ?? throw new System.Exception("A user must be logged in");
            Item bidItem =
                portalContext.Items
                    .Where(item => item.Id == itemId)
                    .FirstOrDefault()
                ?? throw new System.Exception(string.Format("The item (id={0}) does not exist", itemId));
            
            Bid newBid =  new Bid 
            {
                Amount = amount,
                Item = bidItem,
                User = currentUser
            };

            int? currentBid = portalContext.Bids
                .Include(bid => bid.Item)
                .Where(bid => bid.Item == bidItem)
                .OrderByDescending(bid => bid.Amount)
                .Select(bid => (int?)bid.Amount)
                .FirstOrDefault();
            if (
                (currentBid is null && amount == bidItem.InitLicit) ||
                (currentBid != null && amount > currentBid)
            ) {
                portalContext.Bids.Add(newBid);
                portalContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Portal.Persistence;
using Portal.Persistence.DTO;

namespace Portal.API.Services
{
    public class PublishingService
    {
        private readonly PortalContext context;
        private readonly HttpContext httpContext;
        private readonly UserManager<DbUser> userManager;

        public PublishingService(PortalContext context, IHttpContextAccessor contextAccessor, UserManager<DbUser> userManager)
        {
            this.context = context;
            this.httpContext = contextAccessor.HttpContext;
            this.userManager = userManager;
        }

        public async Task<bool> CloseItem(int itemId)
        {
            var item =
                context.Items
                    .Include(it => it.Bids)
                    .Where(it => it.Id == itemId && it.Bids.Any())
                    .FirstOrDefault();
            if (item is null)
            {
                return false;
            }
            else
            {
                item.Expiration = System.DateTime.Now;
                await context.SaveChangesAsync();
                return true;
            }
        }
        public async Task<ItemPreviewDTO[]> GetPreviews()
        {
            var user = await userManager.GetUserAsync(httpContext.User);
            return context.Items
                .Include(item => item.Bids)
                .Include(item => item.Publisher)
                .Where(item => item.Publisher == user)
                .OrderByDescending(item => item.Expiration)
                .Select(item =>
                    new ItemPreviewDTO()
                    {
                        Id = item.Id,
                        Category = item.Category.Name,
                        Name = item.Name,
                        CurrentBid =
                            item.Bids.Any()
                                ? (int?)item.Bids.Select(bid => bid.Amount).Max()
                                : null
                    }
                )
                .ToArray();
        }

        public async Task<ItemDataDTO> GetItem(int itemId)
        {
            var user = await userManager.GetUserAsync(httpContext.User);
            var categories = await context.Categories
                .Select(cat => cat.Name)
                .ToArrayAsync();
            var item = await context.Items
                .Include(it => it.Bids)
                .ThenInclude(bid => bid.User)
                .Include(it => it.Category)
                .Where(it => it.Id == itemId && it.Publisher == user)
                .Select(it =>
                    new ItemDataDTO()
                    {
                        Categories = categories,
                        Id = it.Id,
                        Name = it.Name,
                        Category = it.Category.Name,
                        Description = it.Description,
                        InitLicit = it.InitLicit,
                        Image = it.Image,
                        Bids =
                            it.Bids
                                .OrderByDescending(bid => bid.PutDate)
                                .Select(bid =>
                                    new BidDTO()
                                    {
                                        Amount = bid.Amount,
                                        BuyerName = bid.User.Name,
                                        PutDate = bid.PutDate
                                    }
                                )
                                .ToArray(),
                        Expiration = it.Expiration,
                        PublishDate = it.PublishDate
                    }
                )
                .FirstOrDefaultAsync();
            return item;
        }

        private InsertionResultDTO Error(string msg) =>
            new InsertionResultDTO()
            {
                Id = null,
                Error = msg
            };

        public async Task<InsertionResultDTO> InsertItem(ItemDataDTO itemData)
        {
            if (String.IsNullOrWhiteSpace(itemData.Name))
            {
                return Error("Name must not be empty");
            }
            if (String.IsNullOrWhiteSpace(itemData.Description))
            {
                return Error("Description must not be empty");
            }
            if (itemData.Expiration <= DateTime.Now)
            {
                return Error("Expiration date must be in the future.");
            }
            if (itemData.InitLicit <= 0)
            {
                return Error("Initial licit must be greater than zero.");
            }
            var category = await context.Categories
                .Where(cat => cat.Name == itemData.Category)
                .FirstOrDefaultAsync();
            if(category is null)
            {
                return Error($"Category '{itemData.Category}' does not exist.");
            }
            var publisher =
                await userManager.GetUserAsync(httpContext.User)
                ?? throw new Exception("A publisher must be signed in");
            Item item = new Item()
            {
                Category = category,
                Description = itemData.Description,
                Expiration = itemData.Expiration,
                Image = itemData.Image,
                InitLicit = itemData.InitLicit,
                Name = itemData.Name,
                PublishDate = DateTime.Now,
                Publisher = publisher
            };
            await context.Items.AddAsync(item);
            context.SaveChanges();
            return new InsertionResultDTO()
            {
                Id = item.Id
            };
        }
    }
}
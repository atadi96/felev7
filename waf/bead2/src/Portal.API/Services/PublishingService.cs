using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Portal.Persistence;
using Portal.Persistence.DTO;

namespace Portal.API.Services
{
    public class PublishingServices
    {
        private readonly PortalContext context;
        private readonly HttpContext httpContext;
        private readonly UserManager<DbUser> userManager;

        public PublishingServices(PortalContext context, IHttpContextAccessor contextAccessor, UserManager<DbUser> userManager)
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
            if(item is null)
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
            return await context.Items
                .Include(item => item.Bids)
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
                                ? (int?)item.Bids.Select(bid => bid.Amout).Max()
                                : null
                    }
                )
                .ToArrayAsync();
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
                                        Amount = bid.Amout,
                                        BuyerName = bid.User.Name,
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

        //TODO: post new item
    }
}
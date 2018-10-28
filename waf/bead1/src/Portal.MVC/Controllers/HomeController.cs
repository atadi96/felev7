using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.MVC.Models;
using Portal.MVC.Services;

namespace Portal.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly PortalService portalService;

        public HomeController(PortalService portalService)
        {
            this.portalService = portalService;
        }
        public IActionResult Index(HomeViewModel vm)
        {
            vm.UpdatePageContents(portalService.HomeItems(vm.ItemSearch, vm.Category));
            vm.Categories = portalService.CategorySelectList();
            portalService.SetViewDataUser(ViewData);
            return View("Index", vm);
        }

        public IActionResult Item(int id)
        {
            portalService.SetViewDataUser(ViewData);
            return View("Item", portalService.GetItemForId(id));
        }

        public IActionResult Category(string id)
        {
            portalService.SetViewDataUser(ViewData);
            return View("Category", portalService.GetItemsInCategory(id));
        }

        [Authorize]
        public IActionResult MyItems()
        {
            portalService.SetViewDataUser(ViewData);
            return View("MyItems", portalService.MyItems());
        }

        [HttpPost]
        [Authorize]
        public IActionResult PlaceBid([Bind("ItemId", "BidAmount")] PlaceBidViewModel vm)
        {
            try
            {
                bool notRekt = portalService.PlaceBidint(vm.ItemId, vm.BidAmount);
                return RedirectToActionPermanent(nameof(Item), "Home", new { id = vm.ItemId});
            } catch (Exception e)
            {
                return Content(e.Message);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            portalService.SetViewDataUser(ViewData);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

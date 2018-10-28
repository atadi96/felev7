using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Portal.MVC.Models;
using Portal.MVC.Services;
using Portal.Persistence;

namespace Portal.MVC.Controllers
{
    public class BuyerController : Controller
    {
        private readonly BuyerService buyerService;

        public BuyerController(BuyerService buyerService)
        {
            this.buyerService = buyerService;
        }

        private ActionResult EscapeIfLoggedIn()
        {
            if (!(buyerService.CurrentDbUser() is null))
            {
                return RedirectToActionPermanent(nameof(HomeController.Index), "Home");
            }
            else return null;
        }
        public ActionResult Index()
        {
            return RedirectToActionPermanent(nameof(Register));
        }

        public ActionResult Register(BuyerViewModel vm)
        {
            return EscapeIfLoggedIn() ?? View("Register", vm);
        }

        // POST: Buyer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Name", "UserName", "Email", "PhoneNumber", "Password")]BuyerViewModel vm)
        {
            var esc = EscapeIfLoggedIn();
            if(esc is null)
            {
                try
                {
                    if(ModelState.IsValid)
                    {
                        var result = buyerService.Register(vm).Result;
                        if(result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                            return View(nameof(Register), vm);
                        }
                    }
                    else
                    {
                        return View(nameof(Register), vm);
                    }
                }
                catch (Exception e)
                {
                    return Content(e.Message);
                }
            }
            else
            {
                return esc;
            }
        }

        [Authorize]
        public IActionResult Logout(int id)
        {
            buyerService.Logout(id);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
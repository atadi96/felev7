using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Portal.Persistence
{
    public static class DbInitializer
    {
        private static class Categories
        {
            public static Category Instrument;
            public static Category Furniture;
            public static Category Other;
        }
        private static class Publishers
        {
            public static DbUser Instrument;
            public static DbUser Furniture;
        }
        private static class Items
        {
            public static Item LP100;
            public static Item Amp;
            public static Item Desk;
            public static Item Farewell;
        }
        private static class Users
        {
            public static DbUser CheapBuyer;
        }
        private static UserManager<DbUser> userManager;
        private static PortalContext portalContext;
        public static void Initialize(
            PortalContext context,
            UserManager<DbUser> userManager,
            RoleManager<IdentityRole<int>> roleManager
        ) {
            DbInitializer.userManager = userManager;
            portalContext = context;

            portalContext.Database.EnsureCreated();

            portalContext.Database.Migrate();

            InitRoles(roleManager);

            if (portalContext.Items.Any())
            {
                return;
            }
            else
            {
                Init();
                portalContext.SaveChanges();
            }
        }
        
        private static void InitRoles(RoleManager<IdentityRole<int>> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                var x = roleManager.CreateAsync(new IdentityRole<int>("Buyer")).Result;
                var y = roleManager.CreateAsync(new IdentityRole<int>("Publisher")).Result;
            }
        }

        private static void Init()
        {
            InitCategories();
            Publishers.Furniture = new DbUser
            {
                Name = "Furniture Publisher",
                UserName = "furn"
            };
            Publishers.Instrument = new DbUser
            {
                Name = "Instrument Publisher",
                UserName = "inst"
            };
            Users.CheapBuyer = new DbUser
            {
                UserName = "cheap",
                Name = "Cheap Cheap",
                Email = "ch@ch.ch",
                PhoneNumber = "asdf"
            };

            var result = userManager.CreateAsync(Users.CheapBuyer, "cheap").Result;
            result = userManager.AddToRoleAsync(Users.CheapBuyer, "Buyer").Result;
            result = userManager.CreateAsync(Publishers.Furniture, "asdf").Result;
            result = userManager.AddToRoleAsync(Publishers.Furniture, "Publisher").Result;
            result = userManager.CreateAsync(Publishers.Instrument, "qwer").Result;
            result = userManager.AddToRoleAsync(Publishers.Instrument, "Publisher").Result;

            portalContext.SaveChanges();
            portalContext.Items.AddRange(Picks().Take(50));
            portalContext.SaveChanges();
            InitFurniture();
            InitInstrument();
            portalContext.SaveChanges();
            AddBid();
        }

        private static void AddBid()
        {
            Bid bid = new Bid
            {
                Amount = 10,
                User = Users.CheapBuyer,
                Item = Items.Farewell
            };
            Bid bid2 = new Bid()
            {
                Amount = 5_000,
                User = Users.CheapBuyer,
                Item = Items.Amp
            };
            Bid bid3 = new Bid()
            {
                Amount = 40_000,
                User = Users.CheapBuyer,
                Item = Items.LP100
            };

            portalContext.Bids.Add(bid);
            portalContext.Bids.Add(bid2);
            portalContext.Bids.Add(bid3);
            portalContext.SaveChanges();
        }

        private static void InitCategories()
        {
            Categories.Instrument = new Category
            {
                Name = "Instrument"
            };
            Categories.Furniture = new Category
            {
                Name = "Furniture"
            };
            Categories.Other = new Category
            {
                Name = "Other"
            };
            portalContext.Categories.AddRange(
                new Category[] { Categories.Other, Categories.Instrument, Categories.Furniture }
            );
        }

        private static IEnumerable<Item> Picks()
        {
            while (true)
            {
                yield return new Item
                {
                    Category = Categories.Instrument,
                    Description = "1.0mm pick",
                    Expiration = DateTime.Now.AddHours(1),
                    InitLicit = 200,
                    Name = "Guitar pick",
                    Publisher = Publishers.Instrument
                };
            }
        }

        private static void InitInstrument()
        {
            Items.LP100 = new Item
            {
                Category = Categories.Instrument,
                Name = "Epiphone LP100",
                Description = "Used Epiphone LP100 from 2011, good condition.",
                Expiration = DateTime.Now.AddDays(3),
                InitLicit = 40_000,
                Publisher = Publishers.Instrument,
                Image = File.ReadAllBytes("lp100.jpg"),
            };
            Items.Amp = new Item
            {
                Category = Categories.Instrument,
                Name = "Behringer amplifier",
                Description = "Behringer 15W amplifier, second owner, line input is broken",
                Expiration = DateTime.Now.AddDays(-1),
                InitLicit = 5_000,
                Publisher = Publishers.Instrument,
                Image = null,
            };
            portalContext.Items.AddRange(
                new Item[] { Items.LP100, Items.Amp }
            );
        }

        private static void InitFurniture()
        {
            Items.Desk = new Item
            {
                Category = Categories.Furniture,
                Name = "Wooden desk",
                Description = "Wooden desk, pine, home built",
                Expiration = DateTime.Now.AddDays(4),
                InitLicit = 20_000,
                Publisher = Publishers.Furniture,
                Image = null,
            };
            Items.Farewell = new Item
            {
                Category = Categories.Other,
                Name = "Farewell",
                Description = "old friend",
                Expiration = DateTime.Now.AddDays(1001),
                InitLicit = 1,
                Publisher = Publishers.Furniture,
                Image = null,
            };
            portalContext.Items.AddRange(
                new Item[] { Items.Desk, Items.Farewell }
            );
        }
    }



}
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
            public static Item CheapBuyer;
            public static Item BigBuyer;
        }
        private static PortalContext portalContext;
        public static void Initialize(PortalContext context, UserManager<DbUser> userManager)
        {
            portalContext = context;

            portalContext.Database.EnsureCreated();

            portalContext.Database.Migrate();

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
        private static void Init()
        {
            InitCategories();
            DbUser publisher1 = new DbUser
            {
                Name = "Furniture Publisher",
                UserName = "furn"
            };
            DbUser publisher2 = new DbUser
            {
                Name = "Instrument Publisher",
                UserName = "inst"
            };
            InitFurniture();
            InitInstrument();
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
                Image = null,
            };
            Items.Amp = new Item
            {
                Category = Categories.Instrument,
                Name = "Behringer amplifier",
                Description = "Behringer 15W amplifier, second owner, line input is boken",
                Expiration = DateTime.Now.AddDays(-1),
                InitLicit = 5_000,
                Publisher = Publishers.Instrument,
                Image = null,
            };
            
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
        }
    }



}
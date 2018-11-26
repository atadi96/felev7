using System;
using Portal.Persistence;

namespace Portal.API.Test
{
    internal sealed class DbInitializer
    {
        public static class Categories
        {
            public static Category Instrument;
            public static Category Furniture;
            public static Category Other;
        }
        public static class Publishers
        {
            public static DbUser Instrument;
            public static DbUser Furniture;
        }
        public static class Items
        {
            public static Item LP100;
            public static Item Amp;
            public static Item Desk;
            public static Item Farewell;
        }
        public static class Users
        {
            public static DbUser CheapBuyer;
        }

        static DbInitializer()
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
            Items.LP100 = new Item
            {
                Category = Categories.Instrument,
                Name = "Epiphone LP100",
                Description = "Used Epiphone LP100 from 2011, good condition.",
                Expiration = DateTime.Now.AddDays(3),
                InitLicit = 40_000,
                Publisher = Publishers.Instrument,
                Image = null
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
        }

        public static void ApplyToContext(PortalContext portalContext)
        {
            portalContext.Categories.AddRange(
                new Category[] { Categories.Other, Categories.Instrument, Categories.Furniture }
            );
            portalContext.DbUsers.AddRange(
                new DbUser[] { Publishers.Furniture, Publishers.Instrument, Users.CheapBuyer }
            );
            portalContext.Items.AddRange(
                new Item[] { Items.Desk, Items.Farewell }
            );
            portalContext.Items.AddRange(
                new Item[] { Items.LP100, Items.Amp }
            );
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
    }
}

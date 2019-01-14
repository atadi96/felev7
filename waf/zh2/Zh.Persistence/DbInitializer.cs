using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Zh.Persistence
{
    public static class DbInitializer
    {
        private static class Users
        {
            public static DbUser Instrument;
            public static DbUser Furniture;
        }
        private static class Items
        {
        }
        private static UserManager<DbUser> userManager;
        private static ZhContext zhContext;
        public static void Initialize(
            ZhContext context,
            UserManager<DbUser> userManager,
            RoleManager<IdentityRole<int>> roleManager
        ) {
            DbInitializer.userManager = userManager;
            zhContext = context;

            zhContext.Database.EnsureCreated();

            zhContext.Database.Migrate();

            InitRoles(roleManager);

            if (zhContext.Items.Any())
            {
                return;
            }
            else
            {
                Init();
                zhContext.SaveChanges();
            }
        }
        
        private static void InitRoles(RoleManager<IdentityRole<int>> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                var x = roleManager.CreateAsync(new IdentityRole<int>("Role1")).Result;
                var y = roleManager.CreateAsync(new IdentityRole<int>("Role2")).Result;
            }
        }

        private static void Init()
        {
            Users.Furniture = new DbUser
            {
                Name = "Furniture Publisher",
                UserName = "furn"
            };
            Users.Instrument = new DbUser
            {
                Name = "Instrument Publisher",
                UserName = "inst"
            };

            var result = userManager.CreateAsync(Users.Furniture, "asdf").Result;
            result = userManager.AddToRoleAsync(Users.Furniture, "Role1").Result;
            result = userManager.CreateAsync(Users.Instrument, "qwer").Result;
            result = userManager.AddToRoleAsync(Users.Instrument, "Role1").Result;

            zhContext.SaveChanges();
        }
    }



}
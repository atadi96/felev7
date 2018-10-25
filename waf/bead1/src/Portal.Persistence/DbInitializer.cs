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
        private static PortalContext _context;

        public static void Initialize(PortalContext context, UserManager<DbUser> userManager = null)
        {
            _context = context;

            //_context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Database.Migrate();

            if (_context.Items.Any())
            {
                return;
            }
            else
            {
                _context.SaveChanges();
            }
        }
    }



}
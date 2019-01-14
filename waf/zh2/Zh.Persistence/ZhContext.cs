using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Zh.Persistence
{
    public class ZhContext : IdentityDbContext<DbUser, IdentityRole<int>, int>
    {

        public DbSet<Item> Items { get; set; }

        public DbSet<Thing> Things { get; set; }

        public DbSet<DbUser> DbUsers { get; set; }
        public ZhContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            builder.Entity<Thing>()
                .Property(bid => bid.CreateDate)
                .HasDefaultValueSql("getdate()");
            builder.Entity<DbUser>()
                .HasMany(user => user.Things)
                .WithOne(bid => bid.User)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Item>()
                .Property(item => item.CreateDate)
                .HasDefaultValueSql("getdate()");
        }
    }
}
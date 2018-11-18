using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Portal.Persistence
{
    public class PortalContext : IdentityDbContext<DbUser, IdentityRole<int>, int>
    {

        public DbSet<Item> Items { get; set; }

        public DbSet<Bid> Bids { get; set; }

        public DbSet<DbUser> DbUsers { get; set; }

        public DbSet<Category> Categories { get; set; }

        public PortalContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            builder.Entity<Bid>()
                .Property(bid => bid.PutDate)
                .HasDefaultValueSql("getdate()");
            builder.Entity<DbUser>()
                .HasMany(user => user.Bids)
                .WithOne(bid => bid.User)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Item>()
                .Property(item => item.PublishDate)
                .HasDefaultValueSql("getdate()");
        }
    }
}
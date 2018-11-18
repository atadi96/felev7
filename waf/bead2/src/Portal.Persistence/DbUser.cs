using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Persistence
{
    public class DbUser : IdentityUser<int>
    {
        [Required]
        public string Name { get; set; }

        public HashSet<Bid> Bids { get; set; }

        public DbUser() : base()
        {
            Bids = new HashSet<Bid>();
        }
    }
}
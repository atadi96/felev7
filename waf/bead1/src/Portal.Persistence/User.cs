using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Portal.Persistence
{
    public class User : IdentityUser<int>
    {
        [Required]
        public string Name { get; set; }

        public HashSet<Bid> Bids { get; set; }

        public User() : base()
        {
            Bids = new HashSet<Bid>();
        }
    }
}
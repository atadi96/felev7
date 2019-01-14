using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zh.Persistence
{
    public class DbUser : IdentityUser<int>
    {
        [Required]
        public string Name { get; set; }

        public HashSet<Thing> Things { get; set; }

        public DbUser() : base()
        {
            Things = new HashSet<Thing>();
        }
    }
}
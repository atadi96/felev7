using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Portal.Persistence
{
    public class User : IdentityUser<int>
    {
        [Required]
        public string Name { get; set; }}
    }
}
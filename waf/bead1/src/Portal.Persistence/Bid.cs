using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Persistence
{
    public class Bid
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id;

        [Required]
        public Item Item;

        [Required]
        public User User;

        [Required]
        public int Amout;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Date;
    }
}
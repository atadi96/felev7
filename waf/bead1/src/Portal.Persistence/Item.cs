using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Portal.Persistence
{
    public class Item
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int InitLicit { get; set; }

        public byte[] Image { get; set; }

        [Required]
        public DbUser Publisher { get; set; }

        public HashSet<Bid> Bids { get; set; }

        [Required]
        public DateTime Expiration { get; set; }

        public Item()
        {
            Bids = new HashSet<Bid>();
        }
    }
}
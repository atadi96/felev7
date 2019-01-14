using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Zh.Persistence
{
    public class Item
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public byte[] Image { get; set; }

        [Required]
        public DbUser DbUser { get; set; }

        public HashSet<Thing> Things { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public Item()
        {
            CreateDate = DateTime.Now;
            Things = new HashSet<Thing>();
        }
    }
}
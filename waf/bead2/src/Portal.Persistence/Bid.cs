using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Persistence
{
    public class Bid
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public Item Item { get; set; }

        [Required]
        public DbUser User { get; set; }

        [Required]
        public int Amount { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime PutDate { get; set; }

        public Bid()
        {
            PutDate = DateTime.Now;
        }
    }
}
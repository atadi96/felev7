using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel;

namespace Portal.MVC.Models
{
    public class ItemViewModel
    {
        public string Name { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public int CurrentLicit { get; set; }

        public byte[] Image { get; set; }

        public string Publisher { get; set; }

        [DisplayName("Publish Date")]
        public DateTime PublishDate { get; set; }

        public DateTime Expiration { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Zh.Persistence.DTO
{
    public class ItemDTO
    {
        public int Id { get; set; }

        public byte[] Image { get; set; }

        public DbUser DbUser { get; set; }

        public ThingDTO[] Things { get; set; }

        public DateTime CreateDate { get; set; }

        public ItemDTO()
        {
            CreateDate = DateTime.Now;
            Things = new ThingDTO[0];
        }

        public ItemDTO(Item item)
        {
            Id = item.Id;
            Image = item.Image;
            DbUser = item.DbUser;
            Things = item.Things
                .Select(thing => new ThingDTO(thing))
                .ToArray();
            CreateDate = item.CreateDate;
        }
    }
}
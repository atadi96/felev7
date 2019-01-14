using System;

namespace Zh.Persistence.DTO
{
    public class ThingDTO
    {
        public int Id { get; set; }

        public Item Item { get; set; }

        public DbUser User { get; set; }

        public DateTime CreateDate { get; set; }

        public ThingDTO()
        {
            CreateDate = DateTime.Now;
        }

        public ThingDTO(Thing thing)
        {
            Id = thing.Id;
            Item = thing.Item;
            User = thing.User;
            CreateDate = thing.CreateDate;
        }
    }
}
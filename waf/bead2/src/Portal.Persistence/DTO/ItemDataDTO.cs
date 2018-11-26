using System;

namespace Portal.Persistence.DTO
{
    public class ItemDataDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public int InitLicit { get; set; }

        public byte[] Image { get; set; }

        public BidDTO[] Bids { get; set; }

        public DateTime Expiration { get; set; }
    }
}
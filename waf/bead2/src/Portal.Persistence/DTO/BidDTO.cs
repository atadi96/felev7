using System;

namespace Portal.Persistence.DTO
{
    public class BidDTO
    {
        public string BuyerName { get; set; }
        public int Amount { get; set; }

        public DateTime PutDate { get; set; }
    }
}
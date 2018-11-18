using Portal.Persistence;

namespace Portal.MVC.Models
{
    public class ItemPreviewViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Publisher { get; set; }
        public int CurrentBid { get; set; }

        public ItemPreviewViewModel(Item item, int currentBid)
        {
            Id = item.Id;
            Name = item.Name;
            Publisher = item.Publisher.Name;
            CurrentBid = currentBid;
        }
    }
}
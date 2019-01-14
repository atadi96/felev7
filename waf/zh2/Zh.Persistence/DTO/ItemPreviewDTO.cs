namespace Zh.Persistence.DTO
{
    public class ItemPreviewDTO
    {
        public int Id { get; set; }

        public ItemPreviewDTO(Item item)
        {
            Id = item.Id;
        }
    }
}
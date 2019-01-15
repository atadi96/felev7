using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zh.Persistence.DTO;

namespace Zh.WPF.Persistence
{
    public interface IPortalPersistence
    {
        bool IsLoggedOn { get; }

        Task<IEnumerable<ItemPreviewDTO>> GetUserItemsAsync();

        Task<ItemDTO> GetItemAsync(int itemId);

        Task<InsertionResultDTO> InsertItemAsync(ItemDTO item);

        Task<InsertionResultDTO> UpdateItemAsync(int itemId, ItemDTO item);

        Task<bool> CloseItemAsync(int itemId);

        Task<PublisherDTO> LoginAsync(string userName, string userPassword);

        Task<bool> LogoutAsync();
    }
}

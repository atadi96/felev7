using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portal.Persistence.DTO;

namespace Portal.WPF.Persistence
{
    public interface IPortalPersistence
    {
        bool IsLoggedOn { get; }

        Task<IEnumerable<string>> GetCategories();

        Task<IEnumerable<ItemPreviewDTO>> GetUserItemsAsync();

        Task<ItemDataDTO> GetItemAsync(int itemId);

        Task<InsertionResultDTO> InsertItemAsync(ItemDataDTO article);

        Task<bool> CloseItemAsync(int articleID);

        Task<PublisherDTO> LoginAsync(string userName, string userPassword);

        Task<bool> LogoutAsync();
    }
}

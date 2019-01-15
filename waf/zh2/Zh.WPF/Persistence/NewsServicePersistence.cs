using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Zh.Persistence.DTO;

namespace Zh.WPF.Persistence
{
    public class PortalServicePersistence : IPortalPersistence
    {
        private const string API_ITEMS = "api/items/";

        private const string API_ACCOUNT = "api/account/";

        private HttpClient _client;

        public bool IsLoggedOn { get; private set; }

        public PortalServicePersistence(string baseAddress)
        {
            _client = new HttpClient(); // a szolgáltatás kliense
            _client.BaseAddress = new Uri(baseAddress); // megadjuk neki a címet
            IsLoggedOn = false;
        }

        public async Task<IEnumerable<ItemPreviewDTO>> GetUserItemsAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(API_ITEMS);
                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<ItemPreviewDTO> previews = await response.Content.ReadAsAsync<IEnumerable<ItemPreviewDTO>>();
                    return previews;
                }
                else
                {
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }

        }

        public async Task<ItemDTO> GetItemAsync(int itemId)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(API_ITEMS + itemId);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else if (response.IsSuccessStatusCode)
                {
                    return (await response.Content.ReadAsAsync<ItemDTO>());
                }
                else
                {
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (PersistenceUnavailableException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<InsertionResultDTO> InsertItemAsync(ItemDTO itemDTO)
        {
            try
            {
                using (HttpResponseMessage response = await _client.PostAsJsonAsync(API_ITEMS, itemDTO))
                { // az értékeket azonnal JSON formátumra alakítjuk
                    var result = await response.Content.ReadAsAsync<InsertionResultDTO>();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<bool> CloseItemAsync(int articleID)
        {
            try
            {
                HttpResponseMessage response = await _client.PutAsync(API_ITEMS + articleID, null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<PublisherDTO> LoginAsync(string userName, string userPassword)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(API_ACCOUNT + "login/" + userName + "/" + userPassword);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<PublisherDTO>();
                    IsLoggedOn = true;
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex.Message);
            }
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(API_ACCOUNT + "logout");
                IsLoggedOn = !response.IsSuccessStatusCode;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<InsertionResultDTO> UpdateItemAsync(int itemId, ItemDTO item)
        {
            try
            {
                using (HttpResponseMessage response = await _client.PutAsJsonAsync(API_ITEMS + itemId, item))
                { // az értékeket azonnal JSON formátumra alakítjuk
                    var result = await response.Content.ReadAsAsync<InsertionResultDTO>();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Persistence.DTO;

namespace Portal.WPF.Persistence
{
    class MockupPersistence : INewsPersistence
    {
        public bool IsLoggedOn => throw new NotImplementedException();

        public Task<bool> CloseItemAsync(int articleID)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetCategories()
        {
            throw new NotImplementedException();
        }

        public Task<ItemDataDTO> GetItemAsync(int itemId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ItemPreviewDTO>> GetUserItemsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<InsertionResultDTO> InsertItemAsync(ItemDataDTO article)
        {
            throw new NotImplementedException();
        }

        public Task<PublisherDTO> LoginAsync(string userName, string userPassword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}

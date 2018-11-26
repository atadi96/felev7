using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Portal.Persistence.DTO;

namespace Portal.WPF.Model
{
    public interface INewsModel
    {

        IReadOnlyList<ItemPreviewDTO> ItemPreviews { get; }

        PublisherDTO LoggedInUser { get; }

        bool IsBusy { get; }

        void LoadPreviewsAsync();

        EventHandler<ModelAsyncCompletedEventArgs<IReadOnlyList<ItemPreviewDTO>>> LoadPreviewsAsyncFinished { get; set; }

        void GetItemAsync(int id);

        void CreateItemAsync(ItemPreviewDTO article);

        void CloseItemAsync(int id);

        EventHandler<ModelAsyncCompletedEventArgs<ItemPreviewDTO>> ItemLoadFinished { get; set; }

        void LoginAsync(string userName, string userPassword);

        EventHandler<ModelAsyncCompletedEventArgs<PublisherDTO>> LoginAsyncFinished { get; set; }

        void LogoutAsync();

        EventHandler<ModelAsyncCompletedEventArgs<bool>> LogoutAsyncFinished { get; set; }
    }
}

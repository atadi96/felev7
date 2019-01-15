using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zh.Persistence.DTO;
using Zh.WPF.Persistence;
using Microsoft.Win32;
using System.Windows.Data;
using System.Windows;

namespace Zh.WPF.ViewModel
{
    class ArticleEditViewModel : ViewModelBase
    {
        public class CategoryItem
        {
            public string Name { get; set; }
            public override string ToString()
            {
                return Name;
            }
            public override bool Equals(object obj)
            {
                return string.Equals(Name, ((CategoryItem)obj).Name);
            }
            public override int GetHashCode()
            {
                return Name.GetHashCode();
            }
        }

        private IPortalPersistence model;
        private bool newPost;

        private bool isReady;

        public bool IsReady
        {
            get => isReady;
            private set { isReady = value; OnPropertyChanged(); }
        }

        private ItemDTO itemData;

        private string title = "Loading...";
        public string Title
        {
            get => title;
            private set { title = value; OnPropertyChanged(); }
        }

        private bool newItem = false;
        public bool NewItem
        {
            get => newItem;
            private set { newItem = value; OnPropertyChanged(); }
        }

        public byte[] Image
        {
            get => itemData.Image;
            set { itemData.Image = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ThingDTO> things;
        public ObservableCollection<ThingDTO> Things
        {
            get => things;
            private set
            {
                if (things != value)
                {
                    things = value;
                    OnPropertyChanged();
                }
            }
        }
        /*
        public string ExpirationText
        {
            get
            {
                var text = itemData.Expiration.ToLongDateString() + " " + itemData.Expiration.ToLongTimeString();
                return text;
            }
            set
            {
                if(DateTime.TryParse(value, out DateTime result))
                {
                    itemData.Expiration = result;
                }
            }
        }
        */
        public DelegateCommand BackCommand { get; private set; }

        public EventHandler BackEvent { get; set; }

        public DelegateCommand SaveCommand { get; private set; }

        public DelegateCommand CloseItemCommand { get; private set; }

        public DelegateCommand AddImageCommand { get; private set; }

        public ArticleEditViewModel(IPortalPersistence model, int? itemId = null)
        {
            this.model = model;
            newPost = itemId == null;
            itemData = new ItemDTO();
            BackCommand = new DelegateCommand(_ => BackEvent?.Invoke(this, EventArgs.Empty));
            SaveCommand = new DelegateCommand(_ => Save());
            AddImageCommand = new DelegateCommand(_ => UploadImage());
            CloseItemCommand = new DelegateCommand(_ => CloseItem());
            IsReady = false;
            FetchArticle(itemId);
        }

        private void UploadImage()
        {
            var dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.Filter = "Image files (*.jpg, *.png, *.jpeg, *.bmp)|*.jpg;*.png;*.jpeg;*.bmp|Any files (*.*)|*.*";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == true)
            {
                foreach (string filename in dialog.FileNames)
                {
                    Image = File.ReadAllBytes(filename);
                }
            }
        }

        private async void Save()
        {
            IsReady = false;
            InsertionResultDTO result;
            try
            {
                result = await model.InsertItemAsync(itemData);
                if (!(result.Id is null))
                {
                    OnMessageApplication("Save successful!");
                    BackEvent?.Invoke(this, null);
                }
                else
                {
                    OnMessageApplication($"Save refused by the server! Error: {result.Error}");
                }
            }
            catch (PersistenceUnavailableException ex)
            {
                PersistenceError(ex);
            }
            IsReady = true;
        }

        private async void CloseItem()
        {
            IsReady = false;
            var x = MessageBox.Show("Are you sure you want to close the auction?", "Close auction", MessageBoxButton.YesNo);
            if (x == MessageBoxResult.Yes)
            {
                try
                {
                    bool result = await model.CloseItemAsync(itemData.Id);
                    MessageBox.Show(result ? "Auction closed successfully." : "Auction close failed!");
                    if (result)
                    {
                        BackEvent?.Invoke(this, null);
                    }
                    IsReady = true;
                }
                catch (PersistenceUnavailableException ex)
                {
                    PersistenceError(ex);
                    IsReady = true;
                }
            }
        }

        private async void FetchArticle(int? itemId = null)
        {
            if (itemId is null)
            {
                Title = "Publish new item";
                itemData.CreateDate = DateTime.Now;

                NewItem = true;
                IsReady = true;
            }
            else
            {
                try
                {
                    itemData = await model.GetItemAsync(itemId.Value);
                    Things = new ObservableCollection<ThingDTO>(itemData.Things);
                    NewItem = false;
                    //Title = "Details - " + itemData.Name;
                    //OnPropertyChanged(nameof(Name));
                    //OnPropertyChanged(nameof(Category));
                    //OnPropertyChanged(nameof(Description));
                    //OnPropertyChanged(nameof(InitLicit));
                    //OnPropertyChanged(nameof(Image));
                    //OnPropertyChanged(nameof(ExpirationText));
                    //OnPropertyChanged(nameof(Closeable));
                    IsReady = true;
                }
                catch (PersistenceUnavailableException ex)
                {
                    PersistenceError(ex);
                    IsReady = true;
                }
            }
            
        }

        private void PersistenceError(PersistenceUnavailableException ex)
        {
            OnMessageApplication($"Persistence error: {ex.Message}");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Portal.Persistence.DTO;
using Portal.WPF.Persistence;
using Microsoft.Win32;
using System.Windows.Data;

namespace Portal.WPF.ViewModel
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

        private INewsPersistence model;
        private bool newPost;

        private bool isReady;

        public bool IsReady
        {
            get => isReady;
            private set { isReady = value; OnPropertyChanged(); }
        }

        private ItemDataDTO itemData;

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

        public bool Closeable =>
            !NewItem &&
            Expiration > DateTime.Now &&
            (itemData?.Bids?.Any() ?? false);

        public string Name
        {
            get => itemData.Name;
            set { itemData.Name = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => itemData.Description;
            set { itemData.Description = value; OnPropertyChanged(); }
        }

        public byte[] Image
        {
            get => itemData.Image;
            set { itemData.Image = value; OnPropertyChanged(); }
        }

        public ReadOnlyObservableCollection<CategoryItem> Categories { get; private set; }

        public string Category
        {
            get => itemData.Category;
            set { itemData.Category = value; OnPropertyChanged(); }
        }

        public int InitLicit
        {
            get => itemData.InitLicit;
            set { itemData.InitLicit = value; OnPropertyChanged(); }
        }

        public ObservableCollection<BidDTO> Bids { get; private set; }

        public DateTime PublishDate
        {
            get => itemData.PublishDate;
            set { itemData.PublishDate = value; OnPropertyChanged(); }
        }

        public DateTime Expiration
        {
            get => itemData.Expiration;
            set
            {
                if (itemData.Expiration == value) return;
                itemData.Expiration = value;
                OnPropertyChanged();
            }
        }

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

        public DelegateCommand BackCommand { get; private set; }

        public EventHandler BackEvent { get; set; }

        public DelegateCommand SaveCommand { get; private set; }

        public DelegateCommand AddImageCommand { get; private set; }

        public ArticleEditViewModel(INewsPersistence model, int? itemId = null)
        {
            this.model = model;
            newPost = itemId == null;
            itemData = new ItemDataDTO();
            BackCommand = new DelegateCommand(_ => BackEvent?.Invoke(this, EventArgs.Empty));
            SaveCommand = new DelegateCommand(_ => Save());
            AddImageCommand = new DelegateCommand(_ => UploadImage());
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

        private async void FetchArticle(int? itemId = null)
        {
            var categories = await model.GetCategories();
            Categories =
                    new ReadOnlyObservableCollection<CategoryItem>(
                        new ObservableCollection<CategoryItem>(
                            categories.Select(catName => new CategoryItem() { Name = catName })
                        )
                    );
            if (itemId is null)
            {
                Title = "Publish new item";
                Expiration = DateTime.Now;
                NewItem = true;
                IsReady = true;
            }
            else
            {
                try
                {
                    itemData = await model.GetItemAsync(itemId.Value);
                    Bids = new ObservableCollection<BidDTO>(itemData.Bids);
                    NewItem = false;
                    Title = "Details - " + itemData.Name;
                    OnPropertyChanged(nameof(Name));
                    OnPropertyChanged(nameof(Category));
                    OnPropertyChanged(nameof(Description));
                    OnPropertyChanged(nameof(InitLicit));
                    OnPropertyChanged(nameof(Image));
                    //OnPropertyChanged(nameof(Expiration));
                    OnPropertyChanged(nameof(ExpirationText));
                    OnPropertyChanged(nameof(PublishDate));
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

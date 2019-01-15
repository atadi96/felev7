using Zh.WPF.Persistence;
using Zh.Persistence.DTO;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Zh.WPF.ViewModel
{
    /// <summary>
    /// A nézetmodell típusa.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private IPortalPersistence _model;
        private ObservableCollection<ItemPreviewDTO> _itemPreviews;
        private bool _isLoaded;
        private PublisherDTO _publisher;

        public ObservableCollection<ItemPreviewDTO> ItemPreviews
        {
            get { return _itemPreviews; }
            private set
            {
                if (_itemPreviews != value)
                {
                    _itemPreviews = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsLoaded
        {
            get { return _isLoaded; }
            private set
            {
                if (_isLoaded != value)
                {
                    _isLoaded = value;
                    OnPropertyChanged();
                }
            }
        }

        public PublisherDTO Publisher
        {
            get { return _publisher; }
            private set
            {
                _publisher = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand CreateArticleCommand { get; private set; }

        public DelegateCommand EditArticleCommand { get; private set; }

        public DelegateCommand ExitCommand { get; private set; }

        public event EventHandler ExitApplication;

        public event EventHandler CreateArticle;

        public event EventHandler<int> EditArticle;

        public MainViewModel(IPortalPersistence model, PublisherDTO author)
        {
            _model = model ?? throw new ArgumentNullException("model");
            _isLoaded = false;
            _publisher = author;

            EditArticleCommand = new DelegateCommand(param => EditArticleAction((int)param));

            ExitCommand = new DelegateCommand(_ => ExitApplication?.Invoke(this, EventArgs.Empty));

            CreateArticleCommand = new DelegateCommand(_ => CreateArticleAction());

            Refresh();
        }

        private void CreateArticleAction()
        {
            IsLoaded = false;
            CreateArticle?.Invoke(this, EventArgs.Empty);
            Refresh();
        }

        private void EditArticleAction(int articleID)
        {
            IsLoaded = false;
            EditArticle?.Invoke(this, articleID);
            Refresh();
        }

        private async void Refresh()
        {
            IsLoaded = false;
            ItemPreviews = new ObservableCollection<ItemPreviewDTO>(await _model.GetUserItemsAsync());
            IsLoaded = true;
        }

        private void PersistenceError(PersistenceUnavailableException ex)
        {
            OnMessageApplication($"Persistence unavailable: {ex.Message}");
        }
    }
}

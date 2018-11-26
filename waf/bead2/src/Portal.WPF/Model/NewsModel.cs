using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Portal.Persistence.DTO;
using Portal.WPF.Persistence;

namespace Portal.WPF.Model
{ /*
    public class NewsModel : INewsModel
    {
        private INewsPersistence newsPersistence;

        private object lockObj = new object();

        public NewsModel(INewsPersistence newsPersistence)
        {
            this.newsPersistence = newsPersistence;
        }

        public IReadOnlyList<ItemPreviewDTO> ItemPreviews { get; private set; }

        public PublisherDTO LoggedInUser { get; private set; }

        public bool IsBusy { get; private set; }

        public EventHandler<ModelAsyncCompletedEventArgs<IReadOnlyList<ItemPreviewDTO>>> LoadPreviewsAsyncFinished { get; set; }
        public EventHandler<ModelAsyncCompletedEventArgs<ItemDataDTO>> ArticleLoadFinished { get; set; }
        public EventHandler<ModelAsyncCompletedEventArgs<bool>> ItemCloseFinished { get; set; }
        public EventHandler<ModelAsyncCompletedEventArgs<AuthorDTO>> LoginAsyncFinished { get; set; }
        public EventHandler<ModelAsyncCompletedEventArgs<bool>> LogoutAsyncFinished { get; set; }

        private class Lock
        {
            public Lock(NewsModel model)
            {
                lock (model.lockObj)
                {
                    if (model.IsBusy)
                    {
                        return false;
                    }
                    else
                    {
                        IsBusy = true;
                        return true;
                    }
                }
            }
        }

        private bool AcquireBusy()
        {
            lock (lockObj)
            {
                if (IsBusy)
                {
                    return false;
                }
                else
                {
                    IsBusy = true;
                    return true;
                }
            }
        }

        private void ReleaseBusy()
        {
            lock(lockObj)
            {
                IsBusy = false;
            }
        }

        public void CreateArticleAsync(ArticleDTO article)
        {
            if (AcquireBusy())
            {
                
            }
        }

        public void DeleteArticleAsync(ArticleDTO building)
        {
            throw new NotImplementedException();
        }

        public void GetItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void LoadPreviewsAsync()
        {
            throw new NotImplementedException();
        }

        public void LoginAsync(string userName, string userPassword)
        {
            throw new NotImplementedException();
        }

        public void LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public void UpdateArticleAsync(ArticleDTO article)
        {
            throw new NotImplementedException();
        }
    } */
}

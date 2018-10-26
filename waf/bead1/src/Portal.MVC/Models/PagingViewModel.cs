using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RipSeiko.Models.Common
{
    public class PagingViewModel<T>
    {
        public PagingViewModel(int itemsPerPage)
        {
            Page = 1;
            MaxPage = 1;
            Items = Array.Empty<T>();
            LastPage = true;
            ItemsPerPage = itemsPerPage;
        }

        public T[] Items { get; set; }
        public int Page { get; set; }
        public int MaxPage { get; set; }
        public bool LastPage { get; set; }
        public bool FirstPage => Page <= 1;

        public int ItemsPerPage { get; set; }

        public void UpdatePageContents(IQueryable<T> query)
        {
            long itemNum = query.LongCount();
            int maxPage = (int)((itemNum - 1) / ItemsPerPage) + 1;
            int actualPage = Math.Min(maxPage, Math.Max(1, Page));
            Items = query.Skip((actualPage - 1) * ItemsPerPage)
                         .Take(ItemsPerPage)
                         .ToArray();
            Page = actualPage;
            LastPage = actualPage == maxPage;
            MaxPage = maxPage;
        }
    }
}
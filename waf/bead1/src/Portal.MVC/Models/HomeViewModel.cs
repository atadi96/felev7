using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Portal.Persistence;
using RipSeiko.Models.Common;

namespace Portal.MVC.Models
{
    public class HomeViewModel : PagingViewModel<ItemPreviewViewModel>
    {
        public string Category { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        public string ItemSearch { get; set; }

        public HomeViewModel() : base(20) { }
    }
}
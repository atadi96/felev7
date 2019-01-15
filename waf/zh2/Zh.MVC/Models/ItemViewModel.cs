using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
/*
[Required]
[DisplayName("")]
[DataType(DataType.)]
*/

using Zh.Persistence;

namespace Zh.MVC.Models
{
    public class ItemViewModel
    {
        public int Id { get; set; }


        [DisplayName("")]
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set;}

        //[DataType(DataType.Upload)]
        //public byte[] Image { get; set; }
    }
}
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Portal.MVC.Models
{
    public class BuyerViewModel
    {
        [Required]
        public string Name;

        [Required]
        [DisplayName("Username")]
        public string UserName;

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email;

        [Required]
        [DisplayName("Phone number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber;

        [Required]
        [DataType(DataType.Password)]
        public string Password;
    }
}
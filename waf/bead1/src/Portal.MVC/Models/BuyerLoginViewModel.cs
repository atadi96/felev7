using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Portal.MVC.Models
{
    public class BuyerLoginViewModel
    {
        [Required]
        [DisplayName("Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
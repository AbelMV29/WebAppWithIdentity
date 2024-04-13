using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAppWithIdentity.mvc.ViewModels
{
    public class LoginAppUser
    {
        [Required(ErrorMessage = "Email is required"),EmailAddress(ErrorMessage ="Email is of type incorrect")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Password is required")]
        public string Password { get; set; }
    }
}

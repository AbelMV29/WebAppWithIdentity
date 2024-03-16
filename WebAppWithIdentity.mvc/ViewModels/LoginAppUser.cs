using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAppWithIdentity.mvc.ViewModels
{
    public class LoginAppUser
    {
        [Required,EmailAddress]
        public string Email { get; set; }
        [Required,PasswordPropertyText]
        public string Password { get; set; }
    }
}

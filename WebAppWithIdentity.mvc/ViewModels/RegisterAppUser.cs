using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAppWithIdentity.mvc.ViewModels
{
    public class RegisterAppUser
    {
        [Required(ErrorMessage ="The full name is required")]
        public string FullName { get; set; }
        [Required(ErrorMessage ="The username is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="The email is required"),EmailAddress(ErrorMessage ="The email type that you sends is incorrect")]
        public string Email { get; set; }
        [Required(ErrorMessage ="The password is required"),PasswordPropertyText]
        public string Password { get; set; }
        [Required(ErrorMessage ="Image account is required")]
        public IFormFile ImageAccount { get; set; }
    }
}

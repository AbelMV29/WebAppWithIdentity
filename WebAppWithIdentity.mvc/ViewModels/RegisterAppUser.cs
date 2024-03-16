using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAppWithIdentity.mvc.ViewModels
{
    public class RegisterAppUser
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
        [Required,PasswordPropertyText]
        public string Password { get; set; }
        [Required]
        public IFormFile ImageAccount { get; set; }
    }
}

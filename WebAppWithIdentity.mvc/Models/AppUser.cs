using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppWithIdentity.mvc.Models
{
    public class AppUser
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string? Address { get; set; }
        public string? ImageAccount { get; set; }
        public string IdIdentityUser { get; set; }
        [ForeignKey("IdIdentityUser")]
        public IdentityUser IdentityUser { get; set; }

    }
}

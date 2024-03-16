using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAppWithIdentity.mvc.Models;

namespace WebAppWithIdentity.mvc.Data
{
    public class DefaultDb : IdentityDbContext<IdentityUser>
    {
        public DefaultDb(DbContextOptions<DefaultDb> options) :base(options) { }


        public DbSet<AppUser> AppUsers { get; set; }

    }
}

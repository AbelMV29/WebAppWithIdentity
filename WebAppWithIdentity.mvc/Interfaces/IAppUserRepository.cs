using WebAppWithIdentity.mvc.Models;

namespace WebAppWithIdentity.mvc.Interfaces
{
    public interface IAppUserRepository : IRepository<AppUser>
    {
        public Task<AppUser> GetByIdIdentityUser(string id);
    }
}

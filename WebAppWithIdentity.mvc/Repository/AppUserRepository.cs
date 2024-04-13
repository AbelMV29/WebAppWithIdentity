using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAppWithIdentity.mvc.Data;
using WebAppWithIdentity.mvc.Interfaces;
using WebAppWithIdentity.mvc.Models;

namespace WebAppWithIdentity.mvc.Repository
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly DefaultDb _context;
        public AppUserRepository(DefaultDb context)
        {
            _context = context;
        }
        public async Task<bool> Add(AppUser entity)
        {
            await _context.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(int id)
        {
            var entity = await GetById(id);
            if (entity == null)
            {
                return false; // Entity not found
            }

            _context.AppUsers.Remove(entity);
            return await Save();
        }

        public async Task<IEnumerable<AppUser>> GetAll()
        {
            var result = await _context.AppUsers.Include(x=>x.IdentityUser).ToListAsync();
            return result;
        }

        public async Task<AppUser> GetById(int id)
        {
            var userResult = await _context.AppUsers.FindAsync(id);
            return userResult;
        }

        public async Task<AppUser> GetByIdIdentityUser(string idIdentityUser)
        {
            var userResult = await _context.AppUsers.Include(appUser=>appUser.Address).FirstOrDefaultAsync(appUser => appUser.IdIdentityUser == idIdentityUser);
            return userResult;
        }
        public async Task<bool> Save()
        {
            var resultSaved = await _context.SaveChangesAsync();
            return resultSaved>0?true : false;
        }

        public async Task<bool> Update(AppUser entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return await Save();
        }
    }
}

using Microsoft.AspNetCore.Identity;

namespace WebAppWithIdentity.mvc.Seed
{
    public class SeedAddRolesFirstTime
    {
        private RoleManager<IdentityRole> _roleManager;
        public SeedAddRolesFirstTime(RoleManager<IdentityRole> roleManager) 
        {
            _roleManager = roleManager;
        }

        public async Task InitRoles()
        {
            var roleUser = new IdentityRole("User");
            await _roleManager.CreateAsync(roleUser);

            var roleAdmin = new IdentityRole("Admin");
            await _roleManager.CreateAsync(roleAdmin);
        }
    }
}

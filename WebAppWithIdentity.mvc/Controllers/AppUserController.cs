using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAppWithIdentity.mvc.Interfaces;
using WebAppWithIdentity.mvc.Models;

namespace WebAppWithIdentity.mvc.Controllers
{
    public class AppUserController : Controller
    {
        private readonly IAppUserRepository _appUserRepository;
        public AppUserController(IAppUserRepository appUserRepository) 
        {
            _appUserRepository = appUserRepository;
        }
        [Authorize(Policy ="Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var appUser = await _appUserRepository.GetById(id);
            return View(appUser);
        }
    }
}

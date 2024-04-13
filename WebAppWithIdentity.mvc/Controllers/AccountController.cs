using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAppWithIdentity.mvc.Interfaces;
using WebAppWithIdentity.mvc.Models;
using WebAppWithIdentity.mvc.ViewModels;

namespace WebAppWithIdentity.mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;    
        private readonly IAppUserRepository _appUserRepository;
        private IPhotoService _photoService;
        private RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<IdentityUser> userManager,IAppUserRepository appUserRepository
            , IPhotoService photoService, RoleManager<IdentityRole> roleManager,SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _appUserRepository = appUserRepository;
            _photoService = photoService;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            var usersResult = await _appUserRepository.GetAll();
            return View(usersResult);
        }
        //[HttpGet]
        //public async Task<IActionResult> CreateRoles()
        //{
        //    var roleUser = new IdentityRole("User");
        //    await _roleManager.CreateAsync(roleUser);

        //    var roleAdmin = new IdentityRole("Admin");
        //    await _roleManager.CreateAsync(roleAdmin);
        //    return View("Works");
        //}

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if(User.Identity.IsAuthenticated)
            {
                return View("Works");
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Register(RegisterAppUser registerAppUser)
        {
            if(!(ModelState.IsValid))
            {
                return View(registerAppUser);
            }
            var userAlreadyExists = await _userManager.FindByEmailAsync(registerAppUser.Email);
            if(userAlreadyExists is not null)
            {
                TempData["Error"] = "The email is already in use";
                return View(registerAppUser);
            }
            var userIdentity = new IdentityUser
            {
                UserName = registerAppUser.UserName,
                Email = registerAppUser.Email
            };

            var resultCreateIdentity = await _userManager.CreateAsync(userIdentity, registerAppUser.Password);
            if (resultCreateIdentity.Succeeded)
            {
                await _userManager.AddToRoleAsync(userIdentity, "Admin");
                var resultAddImageCloud = await _photoService.AddPhotoAsync(registerAppUser.ImageAccount);
                AppUser appUser = new AppUser
                {
                    FullName = registerAppUser.FullName,
                    IdentityUser = userIdentity,
                    ImageAccount = resultAddImageCloud.Url.ToString(),
                    IdIdentityUser = userIdentity.Id
                };

                var resultOfAddUser = await _appUserRepository.Add(appUser);

                if (!resultOfAddUser)
                {
                    TempData["Error"] = "Error to save the appuser account";
                    return View();
                }

                return View("Works");
            }
            else
            {
                TempData["Error"] = "Error to create an account";
                return View();
            }
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginAppUser loginAppUser)
        {
            if(!ModelState.IsValid)
            {
                return View(loginAppUser);
            }
            var userIdentity = await _userManager.FindByEmailAsync(loginAppUser.Email);
            if(userIdentity is null || !(await _userManager.CheckPasswordAsync(userIdentity,loginAppUser.Password)))
            {
                TempData["Error"] = "Email or Password incorrect";
                return View(loginAppUser);
            }
            var resultTrySignInWithPassword = await _signInManager.PasswordSignInAsync(userIdentity, loginAppUser.Password, false, false);
            if (resultTrySignInWithPassword.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            TempData["Error"] = "Error to login";
            return View(loginAppUser);
        }

        public async Task<IActionResult> LogOut(IdentityUser identityUser)
        {
            if (identityUser is null)
            {
                return View("Account/Register");
            }
             await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}

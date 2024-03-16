﻿using CloudinaryDotNet.Actions;
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
        private readonly IAppUserRepository _appUserRepository;
        private IPhotoService _photoService;
        private RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<IdentityUser> userManager,IAppUserRepository appUserRepository, IPhotoService photoService, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _appUserRepository = appUserRepository;
            _photoService = photoService;
            _roleManager = roleManager;
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
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterAppUser registerAppUser)
        {
            if(!ModelState.IsValid)
            {
                return View(ModelState);
            }

            var userIdentity = new IdentityUser
            {
                UserName = registerAppUser.UserName,
                Email = registerAppUser.Email
            };

            var resultCreateIdentity = await _userManager.CreateAsync(userIdentity, registerAppUser.Password);

            if (resultCreateIdentity.Succeeded)
            {
                await _userManager.AddToRoleAsync(userIdentity, "User");
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
                    return View("", "Error to Add AppUser In Database");
                }

                return View("Works");
            }
            else
            {
                return View("", "Error to create an IdentityUser");
            }
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }
    }
}

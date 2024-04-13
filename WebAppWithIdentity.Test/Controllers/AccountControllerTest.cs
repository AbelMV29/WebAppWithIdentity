using CloudinaryDotNet.Actions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppWithIdentity.mvc.Controllers;
using WebAppWithIdentity.mvc.Interfaces;
using WebAppWithIdentity.mvc.Models;
using WebAppWithIdentity.mvc.ViewModels;

namespace WebAppWithIdentity.Test.Controllers
{
    public class AccountControllerTest
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IAppUserRepository _appUserRepository;
        private IPhotoService _photoService;
        private RoleManager<IdentityRole> _roleManager;
        private readonly AccountController _accountController;
        public AccountControllerTest()
        {
            _userManager = A.Fake<UserManager<IdentityUser>>();
            _appUserRepository = A.Fake<IAppUserRepository>();
            _photoService = A.Fake<IPhotoService>();
            _roleManager = A.Fake<RoleManager<IdentityRole>>();
            _signInManager = A.Fake<SignInManager<IdentityUser>>();
            _accountController = new AccountController(_userManager, _appUserRepository,
                _photoService, _roleManager, _signInManager);
        }

        [Fact]
        public async void AccountController_Register_IsSuccess()
        {
            var registerAppUser = A.Fake<RegisterAppUser>();

            A.CallTo(() => _userManager.FindByEmailAsync(registerAppUser.Email)).Returns(Task.FromResult((IdentityUser)null));
            A.CallTo(() => _userManager.CreateAsync(A<IdentityUser>._,registerAppUser.Password)).Returns(IdentityResult.Success);
            A.CallTo(() => _userManager.AddToRoleAsync(A<IdentityUser>._, "User")).Returns(IdentityResult.Success);
            A.CallTo(() => _photoService.AddPhotoAsync(registerAppUser.ImageAccount)).Returns(new ImageUploadResult
            {
                Url = new System.Uri("http://example.com/image.jpg")
            });
            A.CallTo(() => _appUserRepository.Add(A<AppUser>._)).Returns(true);

            var result = await _accountController.Register(registerAppUser) as ViewResult;

            result.Should().NotBeNull();
            result.ViewName.Should().Be("Works");
            result.Should().BeOfType<ViewResult>();
            result.ViewData.Should().BeEmpty();

        }
        [Fact]
        public async void AccountController_Login_IsSuccess()
        {
            var loginAppUser = A.Fake<LoginAppUser>();
            var identityUserFake = A.Fake<IdentityUser>();

            A.CallTo(() => _userManager.FindByEmailAsync(loginAppUser.Email)).Returns(identityUserFake);
            A.CallTo(() => _userManager.CheckPasswordAsync(A<IdentityUser>._, A<string>._)).Returns(true);
            A.CallTo(() => _signInManager.PasswordSignInAsync(A<IdentityUser>._, loginAppUser.Password, false, false)).Returns(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var result = await _accountController.Login(loginAppUser) as RedirectToActionResult;

            result.Should().NotBeNull();
            result.Should().BeOfType<RedirectToActionResult>();
            result.ActionName.Should().Be("Index");
            result.ControllerName.Should().Be("Home");

        }
    }
}

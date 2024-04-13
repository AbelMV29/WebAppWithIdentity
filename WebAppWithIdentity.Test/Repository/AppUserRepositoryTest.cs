using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppWithIdentity.mvc.Controllers;
using WebAppWithIdentity.mvc.Data;
using WebAppWithIdentity.mvc.Interfaces;
using WebAppWithIdentity.mvc.Models;
using WebAppWithIdentity.mvc.Repository;

namespace WebAppWithIdentity.Test.Repository
{
    public class AppUserRepositoryTest
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly DefaultDb _context;
        private DbContextOptions<DefaultDb> _options;
        private readonly AccountController _accountController;
        public AppUserRepositoryTest()
        {
            _options = new DbContextOptionsBuilder<DefaultDb>().UseInMemoryDatabase(databaseName:Guid.NewGuid().ToString()).Options;
            _context = new DefaultDb(_options);
            _appUserRepository = new AppUserRepository(_context);
            _context.Database.EnsureCreated();
            _context.SaveChanges();

        }

        [Fact]
        public async void AppUserRepository_Get_AllAsync_ReturnListOfTypeAppUser()
        {
            var resultList = await _appUserRepository.GetAll();

            resultList.Should().BeOfType<List<AppUser>>();
        }

    }
}

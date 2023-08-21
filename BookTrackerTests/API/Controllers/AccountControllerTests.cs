using API.Controllers;
using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Persistence;

namespace BookTrackerTests.API.Controllers
{
    public class AccountControllerTests
    {
        [Fact]
        public async Task CanValidateLoginCredentials()
        {
            var context = new DataContext(TestSetup.CreateNewContextOptions());
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            var userManager = TestSetup.CreateUserManager(context);
            var appUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "user123",
                Email = "test@example.com",
                Forename = "forename",
                Surname = "surname"
            };
            await userManager.CreateAsync(appUser, "password");

            var configMock = new Mock<IConfiguration>();
            var tokenService = new TokenService(configMock.Object);

            var controller = new AccountController(userManager, tokenService);

            var loginDto = new LoginDto 
            { 
                Email = "test@example.com", 
                Password = "password",
            };

            var result = await controller.Login(loginDto);

            var objectResult = Assert.IsType<ActionResult<UserDto>>(result);
            var userDto = Assert.IsType<UserDto>(objectResult.Value);

            Assert.Equal("user123", userDto.Username);
            Assert.Equal("forename", userDto.Forename);
            Assert.Equal("surname", userDto.Surname);
        }
        [Fact]
        public async Task CanInvalidLoginCredentials()
        {
            var context = new DataContext(TestSetup.CreateNewContextOptions());
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            var userManager = TestSetup.CreateUserManager(context);
            var appUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "user123",
                Email = "test@example.com",
                Forename = "forename",
                Surname = "surname"
            };
            await userManager.CreateAsync(appUser, "password");

            var configMock = new Mock<IConfiguration>();
            var tokenService = new TokenService(configMock.Object);

            var controller = new AccountController(userManager, tokenService);

            var loginDto = new LoginDto
            {
                Email = "test@example111111111.com",
                Password = "password",
            };

            var result = await controller.Login(loginDto);

            var actionResult = Assert.IsType<ActionResult<UserDto>>(result);
            var unauthorizedResult = Assert.IsType<UnauthorizedResult>(actionResult.Result);
            Assert.Null(actionResult.Value);
            Assert.Equal(401, unauthorizedResult.StatusCode);

        }

        [Fact]
        public async Task HasToken()
        {
            var context = new DataContext(TestSetup.CreateNewContextOptions());
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            var userManager = TestSetup.CreateUserManager(context);
            var appUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "user123",
                Email = "test@example.com",
                Forename = "forename",
                Surname = "surname"
            };
            await userManager.CreateAsync(appUser, "password");

            var configMock = new Mock<IConfiguration>();
            var tokenService = new TokenService(configMock.Object);

            var controller = new AccountController(userManager, tokenService);

            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Password = "password",
            };

            var result = await controller.Login(loginDto);

            var objectResult = Assert.IsType<ActionResult<UserDto>>(result);
            var userDto = Assert.IsType<UserDto>(objectResult.Value);
            Assert.NotNull(userDto.Token);
        }
    }
}

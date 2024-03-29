﻿using API.Controllers;
using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Persistence;
using System.Security.Claims;

namespace BookTrackerTests.API.Controllers
{
    public class AccountControllerTests
    {
        [Fact]
        public async Task CanValidateLoginCredentials()
        {
            var context = new DataContext(TestSetup.CreateNewContextOptions());
            await context.Database.OpenConnectionAsync();
            await context.Database.EnsureCreatedAsync();

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
            configMock.SetupGet(x => x["TokenKey"]).Returns("super secret key");

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
            await context.Database.OpenConnectionAsync();
            await context.Database.EnsureCreatedAsync();

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
            await context.Database.OpenConnectionAsync();
            await context.Database.EnsureCreatedAsync();

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
            configMock.SetupGet(x => x["TokenKey"]).Returns("super secret key");

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
        [Fact]
        public async Task CanRegisterUser()
        {
            var context = new DataContext(TestSetup.CreateNewContextOptions());
            await context.Database.OpenConnectionAsync();
            await context.Database.EnsureCreatedAsync();

            var userManager = TestSetup.CreateUserManager(context);

            var registerDto = new RegisterDto
            {
                Forename = "Forename",
                Surname = "Surname",
                Email = "test@example.com",
                Username = "username",
                Password = "password123"
            };

            var configMock = new Mock<IConfiguration>();
            configMock.SetupGet(x => x["TokenKey"]).Returns("super secret key");

            var tokenService = new TokenService(configMock.Object);

            var controller = new AccountController(userManager, tokenService);

            var result = await controller.Register(registerDto);

            var objectResult = Assert.IsType<ActionResult<UserDto>>(result);
            var userDto = Assert.IsType<UserDto>(objectResult.Value);

            Assert.Equal("username", userDto.Username);
            Assert.Equal("Forename", userDto.Forename);
            Assert.Equal("Surname", userDto.Surname);
        }
        [Fact]
        public async Task CanDetectAlreadyUsedUsername()
        {
            var context = new DataContext(TestSetup.CreateNewContextOptions());
            await context.Database.OpenConnectionAsync();
            await context.Database.EnsureCreatedAsync();

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

            var registerDto = new RegisterDto
            {
                Forename = "Forename",
                Surname = "Surname",
                Email = "test@example.com",
                Username = "user123",
                Password = "password123"
            };

            var configMock = new Mock<IConfiguration>();
            configMock.SetupGet(x => x["TokenKey"]).Returns("super secret key");

            var tokenService = new TokenService(configMock.Object);

            var controller = new AccountController(userManager, tokenService);

            var result = await controller.Register(registerDto);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Username is already taken", objectResult.Value);
        }
        [Fact]
        public async Task CanDetectAlreadyUsedEmail()
        {
            var context = new DataContext(TestSetup.CreateNewContextOptions());
            await context.Database.OpenConnectionAsync();
            await context.Database.EnsureCreatedAsync();

            var userManager = TestSetup.CreateUserManager(context);
            var appUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "user",
                Email = "test@example.com",
                Forename = "forename",
                Surname = "surname"
            };
            await userManager.CreateAsync(appUser, "password");

            var registerDto = new RegisterDto
            {
                Forename = "Forename",
                Surname = "Surname",
                Email = "test@example.com",
                Username = "user123",
                Password = "password123"
            };

            var configMock = new Mock<IConfiguration>();
            configMock.SetupGet(x => x["TokenKey"]).Returns("super secret key");

            var tokenService = new TokenService(configMock.Object);

            var controller = new AccountController(userManager, tokenService);

            var result = await controller.Register(registerDto);

            var objectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Email is already taken", objectResult.Value);
        }
        [Fact]
        public async Task CanGetCurrentUser()
        {

            var context = new DataContext(TestSetup.CreateNewContextOptions());
            await context.Database.OpenConnectionAsync();
            await context.Database.EnsureCreatedAsync();

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
            configMock.SetupGet(x => x["TokenKey"]).Returns("super secret key");

            var tokenService = new TokenService(configMock.Object);

            var controller = new AccountController(userManager, tokenService);

            var userClaims = new[]
            {
                new Claim(ClaimTypes.Email, appUser.Email),
            };

            var userIdentity = new ClaimsIdentity(userClaims, "test");
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            controller.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = userPrincipal
            };

            var result = await controller.GetCurrentUser();

            var objectResult = Assert.IsType<ActionResult<UserDto>>(result);
            var userDto = Assert.IsType<UserDto>(objectResult.Value);

            Assert.Equal("user123", userDto.Username);
            Assert.Equal("forename", userDto.Forename);
            Assert.Equal("surname", userDto.Surname);
        }
    }
}

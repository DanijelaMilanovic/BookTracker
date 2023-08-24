﻿using Infrastructure.Security;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace BookTrackerTests.Infrastructure.Security
{
    public class UserAccessorTests
    {
        [Fact]
        public void CanGetUsername()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "user123")
            };

            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext.User).Returns(claimsPrincipal);

            var userAccessor = new UserAccessor(httpContextAccessorMock.Object);

            // Act
            var username = userAccessor.GetUsername();

            // Assert
            Assert.Equal("user123", username);
        }
    }
}

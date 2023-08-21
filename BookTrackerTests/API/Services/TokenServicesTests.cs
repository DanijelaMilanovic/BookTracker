using API.Services;
using Domain;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTrackerTests.API.Services
{
    public class TokenServicesTests
    {
        [Fact]
        public void CreateToken_ValidUser_ReturnsTokenString()
        {
            var user = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "user",
                Email = "test@example.com"
            };

            var configMock = new Mock<IConfiguration>();
            var tokenService = new TokenService(configMock.Object);

            var token = tokenService.CreateToken(user);

            Assert.NotEmpty(token);
        }
    }
}

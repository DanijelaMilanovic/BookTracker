using API.Services;
using Domain;
using Microsoft.Extensions.Configuration;
using Moq;

namespace BookTrackerTests.API.Services
{
    public class TokenServicesTests
    {
        [Fact]
        public void CreateToken_ValidUser_ReturnsTokenString()
        {
            var configMock = new Mock<IConfiguration>();
            configMock.SetupGet(x => x["TokenKey"]).Returns("super secret key");

            var tokenService = new TokenService(configMock.Object);
            var user = new AppUser
            {
                UserName = "user",
                Id = "123",
                Email = "test@example.com"
            };

            var token = tokenService.CreateToken(user);

            Assert.NotNull(token);

        }
    }
}

using Infrastructure.Security;
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
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, "user123")
            };

            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext!.User).Returns(claimsPrincipal);

            var userAccessor = new UserAccessor(httpContextAccessorMock.Object);

            var username = userAccessor.GetUsername();

            Assert.Equal("user123", username);
        }
    }
}

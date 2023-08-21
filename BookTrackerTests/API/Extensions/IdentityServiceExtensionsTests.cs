using API.Extensions;
using API.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace BookTrackerTests.API.Extensions
{
    public class IdentityServiceExtensionsTests
    {
        [Fact]
        public void AddApplicationServices_AddsServicesAndConfigurations()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddIdentityServices(configuration);

            var serviceProvider = services.BuildServiceProvider();

            //Assert.Contains(serviceProvider.GetServices<IUserStore<AppUser>>(), provider => provider is IUserStore<AppUser>);
            //Assert.Contains(serviceProvider.GetServices<IUserPasswordStore<AppUser>>(), provider => provider is IUserPasswordStore<AppUser>);
            //Assert.Contains(serviceProvider.GetServices<IUserEmailStore<AppUser>>(), provider => provider is IUserEmailStore<AppUser>);

            Assert.Contains(serviceProvider.GetServices<TokenService>(), provider => provider is TokenService);
        }
    }
}

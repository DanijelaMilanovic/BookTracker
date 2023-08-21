using API.Extensions;
using Application.Books;
using Application.Core;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTrackerTests.API.Extensions
{
    public class ApplicationServicesExtensions
    {
        [Fact]
        public void AddApplicationServices_AddsServicesAndConfigurations()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddApplicationServices(configuration);

            var serviceProvider = services.BuildServiceProvider();

            var dc = serviceProvider.GetServices<DataContext>();
            var im = serviceProvider.GetServices<IMediator>();
            var iv = serviceProvider.GetServices<IValidator>();
            var p = serviceProvider.GetServices<Profile>();

            Assert.Contains(serviceProvider.GetServices<DataContext>(), provider => provider is DataContext);
            Assert.Contains(serviceProvider.GetServices<IMediator>(), provider => provider is IMediator);
           // Assert.Contains(serviceProvider.GetServices<IValidator>(), provider => provider is IValidator);
            //Assert.Contains(serviceProvider.GetServices<Profile>(), provider => provider is Profile);
        }
    }
}

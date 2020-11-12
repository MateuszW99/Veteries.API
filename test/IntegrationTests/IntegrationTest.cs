using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Persistence.Domain;
using Veteries.API;
using Veteries.IntegrationTests.Helpers;

namespace Veteries.IntegrationTests
{
    public class IntegrationTest : IDisposable
    {
        protected readonly HttpClient TestClient;
        private readonly IServiceProvider _serviceProvider;
        public IServiceProvider ServiceProvider => _serviceProvider;
        private readonly string _token;

        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DomainDbContext));
                        services.AddDbContext<DomainDbContext>(options => { options.UseInMemoryDatabase(Guid.NewGuid().ToString()); });
                    });
                });

            _serviceProvider = appFactory.Services;

            var db = _serviceProvider.GetRequiredService<DomainDbContext>();
            db.Database.EnsureCreatedAsync();
            TestClient = appFactory.CreateClient();
            _token = ClientHelper.RegisterUser(TestClient);
            db.InitializeDatabaseWithAnimalRecords();
        }

        protected void Authenticate()
        {
            TestClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("bearer", _token);
        }

        public void Dispose()
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<DomainDbContext>();
            context.Database.EnsureDeleted();
        }
    }
}

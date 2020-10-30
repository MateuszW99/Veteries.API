using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Veteries.IntegrationTests.Endpoints
{
    public class AnimalControllerTests : IntegrationTest
    {
        [Fact]
        public async Task Task_GetAll_WithNoAnimals_ReturnsEmptyResponse()
        {
            await AuthenticateAsync();

            var response = await TestClient.GetAsync("api/Animals");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<List<Animal>>()).Should().BeEmpty();
        }
    }
}

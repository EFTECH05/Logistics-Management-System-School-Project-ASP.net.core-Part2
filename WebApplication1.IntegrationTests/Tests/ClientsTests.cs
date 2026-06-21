using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplication1.IntegrationTests.WebApplicationFactory;
using Xunit;

namespace WebApplication1.IntegrationTests.Tests
{
    public class ClientsTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ClientsTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetClients_ShouldReturn200()
        {
            var response = await _client.GetAsync("/api/clients");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
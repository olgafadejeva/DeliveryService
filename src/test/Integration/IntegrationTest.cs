using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryServiceTests.Integration
{
    public class IntegrationTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        public IntegrationTest()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<TestStartup>());
            _client = _server.CreateClient();
        }

        //[Fact]
        public async Task ReturnHelloWorld()
        {
            // Act
            var response = await _client.GetAsync(":44302/Home/Index");
            response.EnsureSuccessStatusCode();
        }
    }
}

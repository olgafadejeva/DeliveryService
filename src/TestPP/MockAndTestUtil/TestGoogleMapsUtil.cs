using DeliveryService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace DeliveryServiceTests.MockAndTestUtil
{
    public class TestGoogleMapsUtil : IGoogleMapsUtil
    {

        public List<HttpResponseMessage> responses { get; set; }
        public int counter;
        public TestGoogleMapsUtil(List<HttpResponseMessage> responses) {
            this.responses = responses;
            counter = 0;

        }

        public Task<HttpResponseMessage> performGoogleMapsRequestAsync(string uri)
        {
            var result = Task.FromResult(responses[counter]);
            counter++;
            return result;
        }
    }
}

using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DeliveryService.Services
{
    public class GoogleMapsUtil
    {
        public AppProperties options { get; set; } 

        public GoogleMapsUtil(IOptions<AppProperties> optionsAccessor)
        {
            options = optionsAccessor.Value;
        }

        public GoogleMapsUtil() { }

        public virtual async Task<HttpResponseMessage> createDistanceUriAndGetResponse(string currentLocationString, string pickUpAddressString, HttpClient httpClient)
        {
            string uri = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=" + currentLocationString + "&destinations=" + pickUpAddressString + "&key=" + options.GoogleMapsApiKey;
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            return response;
        }
    }
}

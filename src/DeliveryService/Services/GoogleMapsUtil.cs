using DeliveryService.Models.Entities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
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

        public virtual async Task addLocationDataToAddress(Address address) {
            HttpClient httpClient = new HttpClient();
            string addressString =  DirectionsService.getStringFromAddress(address);
            string uri = "https://maps.googleapis.com/maps/api/geocode/json?address=" + addressString + "&key=" + options.GoogleMapsApiKey;
            HttpResponseMessage response = await httpClient.GetAsync(uri);

            var jsonString = response.Content.ReadAsStringAsync().Result;
            JObject json = JObject.Parse(jsonString);
            var latValue = (string)json.SelectToken("results[0].geometry.location.lat");
            var lat = Convert.ToDouble(latValue);

            var lngValue = (string)json.SelectToken("results[0].geometry.location.lng");
            var lng = Convert.ToDouble(lngValue);

            address.Lat = lat;
            address.Lng = lng;
        }
    }
}

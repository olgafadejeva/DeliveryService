using DeliveryService.Models.Entities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DeliveryService.Controllers.ShipperControllers;

namespace DeliveryService.Services
{
    public class LocationService
    {
        public IGoogleMapsUtil googleMaps { get; set; }

        public LocationService(IGoogleMapsUtil maps)
        {
            this.googleMaps = maps;
        }

        public LocationService() { }

        public virtual async Task<HttpResponseMessage> createDistanceUriAndGetResponse(string currentLocationString, string pickUpAddressString, HttpClient httpClient)
        {
            string uri = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=" + currentLocationString + "&destinations=" + pickUpAddressString;
            HttpResponseMessage response = await googleMaps.performGoogleMapsRequestAsync(uri);
            return response;
        }

        public virtual async Task addLocationDataToAddress(Address address) {
            string addressString =  DirectionsService.getStringFromAddress(address);
            string uri = "https://maps.googleapis.com/maps/api/geocode/json?address=" + addressString;
            HttpResponseMessage response = await googleMaps.performGoogleMapsRequestAsync(uri);

            var jsonString = response.Content.ReadAsStringAsync().Result;
            JObject json = JObject.Parse(jsonString);
            var latValue = (string)json.SelectToken("results[0].geometry.location.lat");
            var lat = Convert.ToDouble(latValue);

            var lngValue = (string)json.SelectToken("results[0].geometry.location.lng");
            var lng = Convert.ToDouble(lngValue);

            address.Lat = lat;
            address.Lng = lng;
        }

        public virtual async Task<PickUpAddress> FindClosestDepotLocationForRoute(ICollection<PickUpAddress> pickUpLocations, Center center)
        {
            var distances = new double[pickUpLocations.Count()];
            for (int i = 0; i < pickUpLocations.Count(); i++)
            {
                string addressString = DirectionsService.getStringFromAddress(pickUpLocations.ElementAt(i));
                string uri = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=" + center.lat + "," + center.lng + "&destinations=" + addressString;
                HttpResponseMessage response = await googleMaps.performGoogleMapsRequestAsync(uri);

                var jsonString = response.Content.ReadAsStringAsync().Result;
                JObject json = JObject.Parse(jsonString);
                var distance = Convert.ToDouble((string)json.SelectToken("rows[0].elements[0].distance.value"));
                distances[i] = distance;
            }
            var minDistanceIndex = distances.ToList().IndexOf(distances.Min());

            return pickUpLocations.ElementAt(minDistanceIndex);
        }
    }
}

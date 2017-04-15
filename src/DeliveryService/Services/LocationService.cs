using DeliveryService.Models.Entities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DeliveryService.Controllers.ShipperControllers;
using DeliveryService.Models;

namespace DeliveryService.Services
{
    /*
     * Helper service that utilizes Google Maps and is used for:
     *      Checking which depot location is most suitable for a route 
     *      Adding location coordinates to addresses (for displaying markers on the map) 
     *      Getting route distance and completion time 
     *       Finding distance between two addresses
     */       

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

        public virtual async Task<double>  getDistanceBetweenTwoAddresses(Address addressOne, Address addressTwo)
        {
            string addressOneString = DirectionsService.getStringFromAddress(addressOne);
            string addressTwoString = DirectionsService.getStringFromAddress(addressTwo);

            string uri = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=" + addressOneString + "&destinations=" + addressTwoString;
            HttpResponseMessage response = await googleMaps.performGoogleMapsRequestAsync(uri);

            var jsonString = response.Content.ReadAsStringAsync().Result;
            JObject json = JObject.Parse(jsonString);
            var distanceValue = (string)json.SelectToken("rows[0].elements[0].distance.text");
            if (distanceValue == null) // can be NOT FOUND status
            {
                return 1000;
            }
            var valueInDouble = Convert.ToDouble(distanceValue.Replace("mi", ""));
            return valueInDouble;
        }

        public virtual async Task<RouteDetails> getRouteDurationAndOverallDistance(PickUpAddress depot, List<Delivery> deliveries) {

            string waypointsUriString = "";
            foreach (Delivery delivery in deliveries) {
                waypointsUriString += DirectionsService.getStringFromAddressInLatLngFormat(delivery.Client.Address);
                waypointsUriString += "|";
            }
            waypointsUriString = waypointsUriString.TrimEnd('|');
            string depotUriString = DirectionsService.getStringFromAddressInLatLngFormat(depot);
            string uri = "https://maps.googleapis.com/maps/api/directions/json?origin=" + depotUriString + "&destination=" + depotUriString +"&waypoints=optimize:true|" + waypointsUriString;
            HttpResponseMessage response = await googleMaps.performGoogleMapsRequestAsync(uri);
            var jsonString = response.Content.ReadAsStringAsync().Result;

            JObject json = JObject.Parse(jsonString);

            var distanceObjects = json["routes"].Children()["legs"].Children()["distance"];
            var distanceValues = distanceObjects.Select(ds => ds["value"]).Values<double>();
            var overallDistanceInKm = Math.Round(distanceValues.Sum() / 1000, 2);

            var durationObjects = json["routes"].Children()["legs"].Children()["duration"];
            var durationValues = durationObjects.Select(ds => ds["value"]).Values<double>();
            var overallDurationInHours = Math.Round(durationValues.Sum() / 3600, 2);

            return new RouteDetails(overallDistanceInKm, overallDurationInHours);
        }
    }
}

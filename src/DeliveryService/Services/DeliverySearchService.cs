using DeliveryService.Data;
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
    public class DeliverySearchService
    {
        public AppProperties options { get; set; }
        public ApplicationDbContext context { get; set; }

        public DeliverySearchService(IOptions<AppProperties> optionsAccessor, ApplicationDbContext context) {
            options = optionsAccessor.Value;
            this.context = context;
        }

        public DeliverySearchService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IList<Delivery>> searchForDeliveriesWithinDistance(double Latitude, double Longtitude, double PickUpWithin, double DeliveryWithin)
        {

            IEnumerable<Address> pickUpAddresses =  context.Addresses.OfType<PickUpAddress>().ToList();
            IEnumerable<Address> allClientAddresses =  context.Addresses.OfType<ClientAddress>().ToList();
            HttpClient httpClient = new HttpClient();


            string currentLocationString = Latitude + "," + Longtitude;
            IList<Delivery> deliveriesSelectedBasedOnPickUpCriteria = new List<Delivery>();
            IList<Delivery> deliveriesSelectedBasedOnDeliveryAddress = new List<Delivery>();

            await SelectDeliveriesBasedOnDistance(PickUpWithin, pickUpAddresses, httpClient, currentLocationString, deliveriesSelectedBasedOnPickUpCriteria);
            await SelectDeliveriesBasedOnDistance(DeliveryWithin+PickUpWithin, allClientAddresses, httpClient, currentLocationString, deliveriesSelectedBasedOnDeliveryAddress);

            IList<Delivery> selectedDeliveries = deliveriesSelectedBasedOnPickUpCriteria
                .Where(d => deliveriesSelectedBasedOnDeliveryAddress.Any(i => deliveriesSelectedBasedOnPickUpCriteria.Contains(i))).ToList();
            
            return selectedDeliveries;


        }

        private async Task SelectDeliveriesBasedOnDistance(double DistanceWithin, IEnumerable<Address> addresses, HttpClient httpClient, string currentLocationString, IList<Delivery> deliveries)
        {
            foreach (Address address in addresses)
            {
                string addressLocationString = DirectionsService.getStringFromAddress(address, true);
                Uri uri = createUri(currentLocationString, addressLocationString);
                HttpResponseMessage response = await httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    double distanceToAddress = GetDistanceFromResponse(response);
                    if (distanceToAddress < DistanceWithin)
                    {
                        Delivery delivery = null;
                        Type type = address.GetType();
                        if (type == typeof(PickUpAddress))
                        {
                             delivery = context.Deliveries.Where(d => d.PickUpAddress.ID == address.ID).Single();
                        }
                        else {
                             delivery = context.Deliveries.Where(d => d.Client.Address.ID == address.ID).Single();
                        }
                        if (delivery.DeliveryStatus.Status == Status.New && delivery.DeliveryStatus.AssignedTo == null)
                        {
                            deliveries.Add(delivery);
                        }
                    }
                }
            }
        }

        private static double GetDistanceFromResponse(HttpResponseMessage response)
        {
            var jsonString = response.Content.ReadAsStringAsync().Result;
            JObject json = JObject.Parse(jsonString);
            var distanceValue = (string)json.SelectToken("rows[0].elements[0].distance.text");
            var valueInDouble = Convert.ToDouble(distanceValue.Replace("mi", ""));
            return valueInDouble;
        }

        private Uri createUri(string currentLocationString, string pickUpAddressString) {
            string uri = "https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins=" + currentLocationString + "&destinations=" + pickUpAddressString + "&key=" + options.GoogleMapsApiKey;
            return new Uri(uri);
        }
    }
}

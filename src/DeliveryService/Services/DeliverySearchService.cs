using DeliveryService.Data;
using DeliveryService.Models.Entities;
using Microsoft.EntityFrameworkCore;
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
        public ApplicationDbContext context { get; set; }
        public LocationService googleMaps { get; set; }

        public double DEFAULT_NON_FOUND_ADDRESS_VALUE { get; }

        public DeliverySearchService(ApplicationDbContext context, LocationService googleMapsUtil) {
            this.context = context;
            DEFAULT_NON_FOUND_ADDRESS_VALUE = -100.00;
            this.googleMaps = googleMapsUtil;
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

            IList<Delivery> selectedDeliveries = deliveriesSelectedBasedOnDeliveryAddress.Intersect(deliveriesSelectedBasedOnPickUpCriteria).ToList();
            
            return selectedDeliveries;


        }

        public async Task<IList<Delivery>> findAllAvailableDeliveries()
        {
            List<Delivery> allDeliveries = context.Deliveries
                                .Include(d => d.DeliveryStatus)
                                 .Include(d => d.Client)
                                 .Include(d => d.Client.Address).ToList();
            List<Delivery> selectedDeliveries = new List<Delivery>();
            foreach (Delivery delivery in allDeliveries) {
                DeliveryStatus status = delivery.DeliveryStatus;
                if (status != null && status.Status == Status.New) {
                    selectedDeliveries.Add(delivery);
                }
            }
            return selectedDeliveries;

        }

        private async Task SelectDeliveriesBasedOnDistance(double DistanceWithin, IEnumerable<Address> addresses, HttpClient httpClient, string currentLocationString, IList<Delivery> deliveries)
        {
            foreach (Address address in addresses)
            {
                string addressLocationString = DirectionsService.getStringFromAddress(address, true);
                HttpResponseMessage response = await googleMaps.createDistanceUriAndGetResponse(currentLocationString, addressLocationString, httpClient);

                if (response.IsSuccessStatusCode)
                {
                    double distanceToAddress = GetDistanceFromResponse(response);
                    if (distanceToAddress != DEFAULT_NON_FOUND_ADDRESS_VALUE && distanceToAddress < DistanceWithin)
                    {
                        Delivery delivery = null;
                        Type type = address.GetType();
                        if (type == typeof(PickUpAddress))
                        {
                            delivery = context.Deliveries
                                .Include(d => d.DeliveryStatus)
                                .SingleOrDefault();
                        }
                        else {
                             delivery = context.Deliveries
                                 .Include(d => d.DeliveryStatus)
                                 .Include(d => d.Client)
                                 .Include(d => d.Client.Address)
                                .Where(d => d.Client.Address.ID == address.ID)
                                .FirstOrDefault();
                        }
                        if (delivery != null)
                        {
                            DeliveryStatus status = delivery.DeliveryStatus;
                            if (status != null && status.Status == Status.New)
                            {
                                deliveries.Add(delivery);
                            }
                        }
                    }
                }
            }
        }

        private double GetDistanceFromResponse(HttpResponseMessage response)
        {
            var jsonString = response.Content.ReadAsStringAsync().Result;
            JObject json = JObject.Parse(jsonString);
            var distanceValue = (string)json.SelectToken("rows[0].elements[0].distance.text");
            if (distanceValue == null) // can be NOT FOUND status
            {
                return DEFAULT_NON_FOUND_ADDRESS_VALUE;
            }
            var valueInDouble = Convert.ToDouble(distanceValue.Replace("mi", ""));
            return valueInDouble;
        }
    }
}

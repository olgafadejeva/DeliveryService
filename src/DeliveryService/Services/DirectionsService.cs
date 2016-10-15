using DeliveryService.Models;
using DeliveryService.Models.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DeliveryService.Services
{
    public class DirectionsService : IDirectionsService
    {
        public AppProperties options { get; set; }

        public DirectionsService(IOptions<AppProperties> optionsAccessor) {
            options = optionsAccessor.Value;
        }

        public DirectionsService() { }
        public Directions getDirectionsFromAddresses(PickUpAddress pickUpAddress, ClientAddress ClientAddress)
        {
            Directions directions = new Directions(getStringFromAddress(pickUpAddress), getStringFromAddress(ClientAddress));
            directions.ApiKey = options.GoogleMapsApiKey;
            return directions;
        }

        public static string getStringFromAddress(Address address, bool noWhitespace) {
            StringBuilder sb = new StringBuilder();
            sb.Append(address.LineOne)
                .Append(" ")
                .Append(address.LineTwo == null ? " " : address.LineTwo)
                .Append(" ")
                .Append(address.City)
                .Append(" ")
                .Append(address.PostCode);

            if (noWhitespace) {
                return Regex.Replace(sb.ToString(), @"\s+", ",");
            }

            return sb.ToString();
        }

        public static string getStringFromAddress(Address address)
        {
           return  getStringFromAddress(address, false);
        }

    }
}

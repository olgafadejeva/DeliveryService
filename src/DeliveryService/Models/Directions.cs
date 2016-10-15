using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Models
{
    public class Directions
    {
        public string To { get; set; }

        public string From { get; set; }

        public string ApiKey { get; set; }

        public Directions(string From, string To) {

            this.From = From;
            this.To = To;
        }
    }
}

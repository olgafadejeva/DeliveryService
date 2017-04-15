using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Services
{
    /*
     * A class used for specifying secrets in storage
     */ 
    public class AppProperties
    { 
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string GoogleMapsApiKey { get; set; }
    }
}

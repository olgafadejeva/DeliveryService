using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DeliveryService.Services
{
    public interface IGoogleMapsUtil
    {
        Task<HttpResponseMessage> performGoogleMapsRequestAsync(string uri);
        
    }
}

using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace DeliveryService.Services
{
    public class GoogleMapsUtil : IGoogleMapsUtil
    {
        public AppProperties options { get; set; }

        public GoogleMapsUtil(IOptions<AppProperties> optionsAccessor)
        {
            options = optionsAccessor.Value;
        }

        public async Task<HttpResponseMessage> performGoogleMapsRequestAsync(string uri) {
            HttpClient httpClient = new HttpClient();
            string finalUri = uri + "&key=" + options.GoogleMapsApiKey;
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            return response;
        }

        //for testing
        public void setAppProperties(AppProperties props) {
            this.options = props;
        }
    }
}

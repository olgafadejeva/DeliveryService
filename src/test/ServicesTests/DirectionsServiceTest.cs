using DeliveryService.Models;
using DeliveryService.Models.Entities;
using DeliveryService.Services;
using Xunit;

namespace DeliveryServiceTests.Services
{
    /*
     * Tests that directions strings are correctly generated
     */
    public class DirectionsServiceTest
    {
        [Fact]
        public void testGetDirectionsNullValue() {
            
            PickUpAddress pickUpAddress = new PickUpAddress("23 London Road", null, "Brighton", "BN2 4PN");
            ClientAddress clientAddress = new ClientAddress("25 London Road", null, "Brighton", "BN2 4PN");
            DirectionsService service = new DirectionsService();
            service.options = new AppProperties();
            Directions result = service.getDirectionsFromAddresses(pickUpAddress, clientAddress);
            Assert.Equal( "23 London Road   Brighton BN2 4PN", result.From);
            Assert.Equal( "25 London Road   Brighton BN2 4PN", result.To);

        }

        [Fact]
        public void testGetDirections()
        {
            PickUpAddress pickUpAddress = new PickUpAddress("23 London Road", "Flat 2", "Brighton", "BN2 4PN");
            ClientAddress clientAddress = new ClientAddress("25 London Road", "Flat 2", "Brighton", "BN2 4PN");
            DirectionsService service = new DirectionsService();
            service.options = new AppProperties();
            Directions result = service.getDirectionsFromAddresses(pickUpAddress, clientAddress);
            Assert.Equal("23 London Road Flat 2 Brighton BN2 4PN", result.From);
            Assert.Equal("25 London Road Flat 2 Brighton BN2 4PN", result.To);

        }
        
    }
}

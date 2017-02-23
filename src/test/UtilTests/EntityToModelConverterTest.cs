using DeliveryService.Models.DriverViewModels;
using DeliveryService.Models.Entities;
using DeliveryService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryServiceTests.UtilTests
{
    public class EntityToModelConverterTest
    {
        [Fact]
        public void testConvertDeliveryToDriverView() {
            Delivery delivery = new Delivery { ID = 2, ItemSize = ItemSize.Small, ItemWeight = 3 };

            Client client = new Client { Email = "client@clients.com", FirstName = "Alex", LastName = "Smith", ID = 1 };
            delivery.Client = client;

            DeliveryStatus status = new DeliveryStatus { Status = Status.FailedDelivery, ReasonFailed = "customer not present", ID = 2 };
            delivery.DeliveryStatus = status;
            delivery.DeliveryStatusID = 2;

            DriverDeliveryView model = EntityToModelConverter.convertDeliveryToDriverView(delivery);
            Assert.Equal(client, model.Client);
            Assert.Equal(status, model.DeliveryStatus);
            Assert.Equal("Failed", model.StatusString);
            Assert.Equal(ItemSize.Small, model.ItemSize);
            Assert.Equal(3, model.ItemWeight);
        }

        [Fact]
        public void testConvertDriverRouteToDisplayViews() {
            Driver driver = new Driver();
            List<Delivery> deliveries = new List<Delivery>();
            Delivery delivery = new Delivery { ID = 2, ItemSize = ItemSize.Small, ItemWeight = 3 }; Client client = new Client { Email = "client@clients.com", FirstName = "Alex", LastName = "Smith", ID = 1 };
            delivery.Client = client;

            DeliveryStatus status = new DeliveryStatus { Status = Status.FailedDelivery, ReasonFailed = "customer not present", ID = 2 };
            delivery.DeliveryStatus = status;
            delivery.DeliveryStatusID = 2;

            deliveries.Add(delivery);


            DateTime deliverByDate = new DateTime(2016, 10, 10);
            DateTime deliveryDate = new DateTime(2016, 10, 9);
            PickUpAddress address = new PickUpAddress { LineOne = "London Road", City = "Brighton", Lat = 33.3, Lng = 33.3, PostCode = "BN2 2NB" };
            Route routeOne = new Route
            {
                OverallTimeRequired = 3,
                OverallDistance = 100,
                DeliverBy = deliverByDate,
                Status = RouteStatus.Completed,
                VehicleID = 5,
                PickUpAddress = address,
                DeliveryDate = deliveryDate,
                Deliveries = deliveries
            };

            Route routeTwo = new Route
            {
                OverallTimeRequired = 3,
                OverallDistance = 100,
                DeliverBy = deliverByDate.AddDays(5),
                Status = RouteStatus.Completed,
                VehicleID = 2,
                PickUpAddress = address,
                DeliveryDate = deliveryDate.AddDays(5),
                Deliveries = deliveries
            };

            driver.Routes.Add(routeOne);
            driver.Routes.Add(routeTwo);

            List<DriverRouteView> model = EntityToModelConverter.convertDriverRouteToDisplayViews(driver);
            Assert.Equal(2, model.Count);
            Assert.Equal(deliveryDate, model.ElementAt(0).DeliveryDate);
            Assert.Equal(deliveryDate.AddDays(5), model.ElementAt(1).DeliveryDate);
        }
    }
}

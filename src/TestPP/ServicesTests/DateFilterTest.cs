using DeliveryService.Models.Entities;
using DeliveryService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryServiceTests.ServicesTests
{
    public class DateFilterTest
    {
        [Fact]
        public void testDeliveriesWithinTwoDays() {
            List<Delivery> deliveries = new List<Delivery>();
            Delivery del1 = new Delivery();
            del1.DeliverBy = DateTime.Now.AddDays(1);
            deliveries.Add(del1);

            Delivery del2 = new Delivery();
            del2.DeliverBy = DateTime.Now.AddDays(4);
            deliveries.Add(del2);

            var result = DateFilter.getDeliveriesWithinDays(deliveries, 2);
            Assert.Equal(1, result.Count);
            Assert.Equal(del1, result.First());
        }

        [Fact]
        public void testDeliveriesWithinSevenDays() {
            List<Delivery> deliveries = new List<Delivery>();
            Delivery del1 = new Delivery();
            del1.DeliverBy = DateTime.Now.AddDays(9);
            deliveries.Add(del1);

            Delivery del2 = new Delivery();
            del2.DeliverBy = DateTime.Now.AddDays(4);
            deliveries.Add(del2);

            Delivery del3 = new Delivery();
            del3.DeliverBy = DateTime.Now.AddDays(7);
            deliveries.Add(del3);

            var result = DateFilter.getDeliveriesWithinDays(deliveries, 7);
            Assert.Equal(2, result.Count);
            Assert.True(result.Contains(del2));
            Assert.True(result.Contains(del3));
        }

        [Fact]
        public void testGetEarlietDeliverByDate() {
            List<Delivery> deliveries = new List<Delivery>();
            Delivery del1 = new Delivery();
            del1.DeliverBy = DateTime.Now.AddDays(9);
            deliveries.Add(del1);

            Delivery del2 = new Delivery();
            del2.DeliverBy = DateTime.Now.AddDays(4);
            deliveries.Add(del2);

            Delivery del3 = new Delivery();
            del3.DeliverBy = DateTime.Now.AddDays(7);
            deliveries.Add(del3);

            var result = DateFilter.getEarliestDeliverByDate(deliveries);
            Assert.Equal(del2.DeliverBy, result);
        }

        [Fact]
        public void testGetEarlietDeliverByDateNull()
        {
            List<Delivery> deliveries = new List<Delivery>();
            Delivery del1 = new Delivery();
            del1.DeliverBy = null;
            deliveries.Add(del1);

            Delivery del2 = new Delivery();
            del2.DeliverBy = DateTime.Now.AddDays(4);
            deliveries.Add(del2);

            Delivery del3 = new Delivery();
            del3.DeliverBy = DateTime.Now.AddDays(7);
            deliveries.Add(del3);

            var result = DateFilter.getEarliestDeliverByDate(deliveries);
            Assert.Equal(del2.DeliverBy, result);
        }
    }
}

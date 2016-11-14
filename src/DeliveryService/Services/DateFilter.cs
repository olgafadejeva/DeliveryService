using DeliveryService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Services
{
    public static class DateFilter
    {
        public static List<Delivery> getDeliveriesWithinDays(IList<Delivery> deliveries, int days) {
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now.AddDays(days);
            var list = deliveries.Where(d => d.DeliverBy <= endDate && d.DeliverBy >= startDate).ToList<Delivery>();
            return list;
        }

        public static DateTime getEarliestDeliverByDate(IList<Delivery> deliveries) {
            var earliestDate = deliveries
               .Min(r => r.DeliverBy);
            return earliestDate.Value;
        }

        public static List<Route> getRoutesWithinDays(List<Route> routes, int days)
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now.AddDays(days);
            var list = routes.Where(d => d.DeliverBy <= endDate && d.DeliverBy >= startDate).ToList<Route>();
            return list;
        }
    }


}

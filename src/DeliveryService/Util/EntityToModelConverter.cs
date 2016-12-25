using DeliveryService.Models.DriverViewModels;
using DeliveryService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Util
{
    public class EntityToModelConverter
    {
        public static List<DriverRouteView> convertDriverRouteToDisplayViews(Driver driver) {

            List<Route> routes = driver.Routes.OrderBy(r => r.DeliveryDate).ToList();
            List<DriverRouteView> viewModels = new List<DriverRouteView>();
            foreach (Route route in routes)
            {
                DriverRouteView model = new DriverRouteView();
                model.ID = route.ID;
                model.RouteStatusString = RouteStatusExtension.DisplayName(route.Status.Value);
                model.OverallDistance = route.OverallDistance;
                model.OverallTimeRequired = route.OverallTimeRequired;
                model.PickUpAddress = route.PickUpAddress;
                model.DeliverBy = route.DeliverBy;
                model.Deliveries = route.Deliveries;
                model.DeliveryDate = route.DeliveryDate;
                model.Vehicle = driver.Vehicles.Where(v => v.ID == route.VehicleID).FirstOrDefault();
                viewModels.Add(model);
            }
            return viewModels;
        }
    }
}

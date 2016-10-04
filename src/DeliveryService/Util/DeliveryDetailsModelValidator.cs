using DeliveryService.Models.ShipperViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.Util
{
    public static class DeliveryDetailsModelValidator
    {
        public static IList<string> ValidateModel(DeliveryDetails deliveryDetails) {
            IList<string> errors = new List<string>();
            if (deliveryDetails.ClientID == 0) {
                errors.Add("ClientID cannot be empty");
            }

            if (deliveryDetails.useDefaultDeliveryAddress && deliveryDetails.PickUpAddress != null) {
                errors.Add("Cannot use both default and custom delivery address");
            }

            if (!deliveryDetails.useDefaultDeliveryAddress && deliveryDetails.PickUpAddress == null) {
                errors.Add("Need to specify delivery address");
            }
            return errors;
        }

        public static bool IsValid(DeliveryDetails deliveryDetails) {
            return ValidateModel(deliveryDetails).Count() == 0;
        }
    }
}

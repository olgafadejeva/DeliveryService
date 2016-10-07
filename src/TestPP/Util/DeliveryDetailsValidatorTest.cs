using DeliveryService.Models.ShipperViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using DeliveryService.Models.Entities;
using DeliveryService.Util;

namespace DeliveryServiceTests.Util
{
    public class DeliveryDetailsValidatorTest
    {
        public bool DeliveryDetailsValidator { get; private set; }

        [Fact]
        public void testModelNoClientID() {
            DeliveryDetails details = new DeliveryDetails();
            details.PickUpAddress = new PickUpAddress();
            details.useDefaultDeliveryAddress = false;
            Assert.False(DeliveryDetailsModelValidator.IsValid(details));
            Assert.True(DeliveryDetailsModelValidator.ValidateModel(details).Contains("ClientID cannot be empty"));
            Assert.Equal(DeliveryDetailsModelValidator.ValidateModel(details).Count(), 1);
        }

        [Fact]
        public void testModelBothAddressOptionsSelected()
        {
            DeliveryDetails details = new DeliveryDetails();
            details.PickUpAddress = new PickUpAddress();
            details.useDefaultDeliveryAddress = true;
            details.ClientID = 1;
            Assert.False(DeliveryDetailsModelValidator.IsValid(details));
            Assert.True(DeliveryDetailsModelValidator.ValidateModel(details).Contains("Cannot use both default and custom delivery address"));
            Assert.Equal(DeliveryDetailsModelValidator.ValidateModel(details).Count(), 1);
        }

        [Fact]
        public void testModelNoAddressOptionSelected()
        {
            DeliveryDetails details = new DeliveryDetails();
            details.PickUpAddress = null;
            details.useDefaultDeliveryAddress = false;
            details.ClientID = 1;
            Assert.False(DeliveryDetailsModelValidator.IsValid(details));
            Assert.True(DeliveryDetailsModelValidator.ValidateModel(details).Contains("Need to specify delivery address"));
            Assert.Equal(DeliveryDetailsModelValidator.ValidateModel(details).Count(), 1);
        }

        [Fact]
        public void testModelValidWithCustomPickUpAddress()
        {
            DeliveryDetails details = new DeliveryDetails();
            details.PickUpAddress = new PickUpAddress();
            details.useDefaultDeliveryAddress = false;
            details.ClientID = 1;
            Assert.True(DeliveryDetailsModelValidator.IsValid(details));
        }

        [Fact]
        public void testModelValidWithDefaultPickUpAddress()
        {
            DeliveryDetails details = new DeliveryDetails();
            details.PickUpAddress = null;
            details.useDefaultDeliveryAddress = true;
            details.ClientID = 1;
            Assert.True(DeliveryDetailsModelValidator.IsValid(details));
        }
    }
}

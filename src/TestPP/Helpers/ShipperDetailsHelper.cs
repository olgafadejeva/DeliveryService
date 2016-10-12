﻿using DeliveryService.Controllers.ShipperControllers;
using DeliveryService.Models;
using DeliveryService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryServiceTests.Helpers
{
    public class ShipperDetailsHelper
    {
        public static async Task<Shipper> createShipperEntity(ShipperController controller)
        {
            var context = controller.getDbContext();
            var shipperEntity = new Shipper();
            var user = context.ApplicationUsers.First<ApplicationUser>();
            shipperEntity.User = user;
            context.Shippers.Add(shipperEntity);
            await context.SaveChangesAsync();
            return shipperEntity;
        }

        public static Team getTeam()
        {
            Team team = new Team();
            team.CompanyName = "ABC";
            team.Description = "The best company";
            return team;
        }

        public static Delivery getDelivery()
        {
            Delivery delivery = new Delivery();
            delivery.PickUpAddress = getDeliveryPickUpAddress();
            return delivery;

        }

        public static PickUpAddress getDeliveryPickUpAddress()
        {
            PickUpAddress pickUpAddress = new PickUpAddress();
            pickUpAddress.LineOne = "LineOneData";
            pickUpAddress.LineTwo = "LineTwoData";
            pickUpAddress.City = "London";
            pickUpAddress.PostCode = "LN2 3LN";
            return pickUpAddress;
        }

        public static Client getClient() {
            Client client = new Client();
            client.Email = Constants.DEFAULT_EMAIL;
            client.FirstName = Constants.DEFAULT_NAME;
            client.LastName = Constants.DEFAULT_NAME;
            client.Address = new ClientAddress();
            return client;
        }
    }
}
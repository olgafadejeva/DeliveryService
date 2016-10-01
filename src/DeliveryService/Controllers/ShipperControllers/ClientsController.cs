using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeliveryService.Data;
using DeliveryService.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using DeliveryService.Controllers.ShipperControllers;
using DeliveryService.Models.ShipperViewModels;

namespace DeliveryService.ShipperControllers
{
    public class ClientsController : ShipperController
    {

        public ClientsController(ApplicationDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
        {
        }

        public async Task<IActionResult> Index()
        {
            if (shipper == null)
            {
                var user = _context.ApplicationUsers.SingleOrDefault(m => m.Id == currentUserId);
                var shipperEntity = new Shipper();
                shipperEntity.User = user;
                _context.Shippers.Add(shipperEntity);
                await _context.SaveChangesAsync();
                shipper = shipperEntity;
            }
            return View(shipper.Clients);
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await getClient(id);
            if (client == null || !shipper.Clients.Contains(client))
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Email,FirstName,LastName,Address")]Client client)
        {
            if (ModelState.IsValid)
            {
                shipper.Clients.Add(client);
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Client client = await getClient(id);

            if (client == null || !shipper.Clients.Contains(client))
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Email,FirstName,LastName,Address")] Client client)
        {
            if (id != client.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var clientEntity = await _context.Clients.SingleOrDefaultAsync(m => m.ID == id);
                    clientEntity.Email = client.Email;
                    clientEntity.FirstName = client.FirstName;
                    clientEntity.LastName = client.LastName;
                    clientEntity.Address.LineOne = client.Address.LineOne;
                    clientEntity.Address.LineTwo = client.Address.LineTwo;
                    clientEntity.Address.City = client.Address.City;
                    clientEntity.Address.PostCode = client.Address.PostCode;

                    _context.Update(clientEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await getClient(id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.SingleOrDefaultAsync(m => m.ID == id);
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.ID == id);
        }

        private async Task<Client> getClient(int? id)
        {
            return await _context.Clients.Include(m => m.Address)
                .SingleOrDefaultAsync(m => m.ID == id);
        }

    }
}

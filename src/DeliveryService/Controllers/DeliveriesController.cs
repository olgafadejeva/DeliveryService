using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeliveryService.Data;
using DeliveryService.Models.Entities;

namespace DeliveryService.Controllers
{
    public class DeliveriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DeliveriesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Deliveries
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Deliveries.Include(d => d.Client);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Deliveries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var delivery = await _context.Deliveries.SingleOrDefaultAsync(m => m.ID == id);
            if (delivery == null)
            {
                return NotFound();
            }

            return View(delivery);
        }

        // GET: Deliveries/Create
        public IActionResult Create()
        {
            ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "Email");
            return View();
        }

        // POST: Deliveries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ClientID")] Delivery delivery)
        {
            if (ModelState.IsValid)
            {
                _context.Add(delivery);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "Email", delivery.ClientID);
            return View(delivery);
        }

        // GET: Deliveries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var delivery = await _context.Deliveries.SingleOrDefaultAsync(m => m.ID == id);
            if (delivery == null)
            {
                return NotFound();
            }
            ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "Email", delivery.ClientID);
            return View(delivery);
        }

        // POST: Deliveries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ClientID")] Delivery delivery)
        {
            if (id != delivery.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(delivery);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryExists(delivery.ID))
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
            ViewData["ClientID"] = new SelectList(_context.Clients, "ID", "Email", delivery.ClientID);
            return View(delivery);
        }

        // GET: Deliveries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var delivery = await _context.Deliveries.SingleOrDefaultAsync(m => m.ID == id);
            if (delivery == null)
            {
                return NotFound();
            }

            return View(delivery);
        }

        // POST: Deliveries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var delivery = await _context.Deliveries.SingleOrDefaultAsync(m => m.ID == id);
            _context.Deliveries.Remove(delivery);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool DeliveryExists(int id)
        {
            return _context.Deliveries.Any(e => e.ID == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeliveryService.Data;
using DeliveryService.Models.Entities;
using Microsoft.AspNetCore.Http;

namespace DeliveryService.Controllers.DriverControllers
{
    public class DriverHolidaysController : DriverController
    {

        public DriverHolidaysController(ApplicationDbContext context, IHttpContextAccessor contextAccessor) : base(context, contextAccessor)
        {
        }

        // GET: DriverHolidays
        public async Task<IActionResult> Index()
        {
            return View(driver.Holidays.ToList());
        }

        // GET: DriverHolidays/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,From,To")] DriverHoliday driverHoliday)
        {
            if (ModelState.IsValid)
            {
                driver.Holidays.Add(driverHoliday);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(driverHoliday);
        }

        // GET: DriverHolidays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var driverHoliday = await _context.DriverHolidays.SingleOrDefaultAsync(m => m.ID == id);
            if (driverHoliday == null)
            {
                return NotFound();
            }
            return View(driverHoliday);
        }

        // POST: DriverHolidays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,From,To")] DriverHoliday driverHoliday)
        {

            if (ModelState.IsValid)
            {
                DriverHoliday hols = driver.Holidays.Where(h => h.ID == driverHoliday.ID).FirstOrDefault();
                hols.To = driverHoliday.To;
                hols.From = driverHoliday.From;
                _context.Update(hols);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(driverHoliday);
        }

        // GET: DriverHolidays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driverHoliday = await _context.DriverHolidays.SingleOrDefaultAsync(m => m.ID == id);
            if (driverHoliday == null)
            {
                return NotFound();
            }

            return View(driverHoliday);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var driverHoliday = await _context.DriverHolidays.SingleOrDefaultAsync(m => m.ID == id);
            driver.Holidays.Remove(driverHoliday);
            _context.DriverHolidays.Remove(driverHoliday);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool DriverHolidayExists(int id)
        {
            return _context.DriverHolidays.Any(e => e.ID == id);
        }
    }
}

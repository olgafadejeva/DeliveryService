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
using DeliveryService.Models;
using DeliveryService.Services;
using System.Security.Claims;
using DeliveryService.Entities;
using DeliveryService.Models.ShipperViewModels;
using DeliveryService.Controllers.ShipperControllers;

namespace DeliveryService.ShipperControllers
{
    public class TeamsController : ShipperController
    {
        private IEmailSender _emailSender;

        public TeamsController(ApplicationDbContext context, IHttpContextAccessor contextAccessor, IEmailSender emailSender) : base(context, contextAccessor)
        {
            _emailSender = emailSender;
        }

        // GET: Teams
        public  IActionResult Index()
        { 
            Team team = shipper.Team;
            return View(team);
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null  || shipper.Team.ID != id)
            {
                return NotFound();
            }

            var team = await _context.Teams.SingleOrDefaultAsync(m => m.ID == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CompanyName,Description")] Team team)
        {
            if (ModelState.IsValid)
            {
                _context.Add(team);
                shipper.Team = team;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(team);
        }

        public async Task<IActionResult> AddDriver(int? id)
        {

            if (id == null || shipper.Team.ID != id)
            {
                return NotFound();
            }

            var team = await _context.Teams.SingleOrDefaultAsync(m => m.ID == id);
            if (team == null)
            {
                return NotFound();
            }
            return View(new DriverDetails());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDriver(int id, [Bind("ID,CompanyName,Description")] Team team, DriverDetails driver)
        {
            if (id != team.ID || shipper.Team.ID != id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {

                //send email to driver to register with the url that will have link to registration
                var callbackUrl = Url.Action("RegisterDriver", "Account", new { teamId=team.ID, email=driver.Email, firstName = driver.FirstName }, "https");
                await _emailSender.SendEmailAsync(driver.Email, "You were invited to register",
                       $"You were invited as a driver,  please register: <a href='{callbackUrl}'>link</a>");
                DriverRegistrationRequest request = new DriverRegistrationRequest();
                request.TeamID = team.ID;
                request.DriverEmail = driver.Email;
                _context.DriverRegistrationRequests.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(team);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDriver(int? id, int? driverId)
        {
            if (id == null || shipper.Team.ID != id)
            {
                return NotFound();
            }

            var team = await _context.Teams.SingleOrDefaultAsync(m => m.ID == id);
            var drivers = team.Drivers;
            foreach (Driver driver in drivers) {
                if (driver.ID == driverId) {
                    team.Drivers.Remove(driver);
                    await _context.SaveChangesAsync();
                    break;
                }
            }
            
            return View(team);
        }



        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || shipper.Team.ID != id)
            {
                return NotFound();
            }

            var team = await _context.Teams.SingleOrDefaultAsync(m => m.ID == id);
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CompanyName,Description")] Team team)
        {
            if (id != team.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var teamEntity = await _context.Teams.SingleOrDefaultAsync(m => m.ID == id);
                    teamEntity.CompanyName = team.CompanyName;
                    teamEntity.Drivers = team.Drivers;
                    teamEntity.Description = team.Description;
                    _context.Update(teamEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.ID))
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
            return View(team);
        }

        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.SingleOrDefaultAsync(m => m.ID == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _context.Teams.SingleOrDefaultAsync(m => m.ID == id);
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.ID == id);
        }

        public IEmailSender getEmailSender()
        {
            return _emailSender;
        }
    }
}

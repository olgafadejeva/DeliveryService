using DeliveryService.Models;
using DeliveryService.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace DeliveryService.Data.Initializer
{
    /*
     * Creates roles in the database as at the start of the development there was no appropriate way to do this 
     * which might have changed by now
     */ 
    public class DatabaseInitializer {

        public async static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
           

            string[] roles = AppRole.getAllRoles();

            RoleManager<IdentityRole> roleManager = serviceProvider.GetService(typeof(RoleManager<IdentityRole>)) as RoleManager<IdentityRole>;
            
            foreach (string role in roles)
            {
                var roleStore = new RoleStore<IdentityRole>(context);

                if (!context.Roles.Any(r => r.Name == role && r.NormalizedName == role.ToUpper()))
                {
                    await roleManager.
                        CreateAsync(new IdentityRole {
                            Name = role,
                            NormalizedName = role.ToUpper()
                        });
                }
            }
           await context.SaveChangesAsync();
        }
    }
}


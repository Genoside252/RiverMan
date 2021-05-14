using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using RiverMan.Models;
using Microsoft.AspNetCore.Identity;

namespace RiverMan.DataAccessLayer
{
    public class DBInitialiser 
    {
        const string adminRole = "Admin";
        const string memberRole = "Member";
        static ServiceType streamingServiceType = null;

        public static void Initialise(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, RiverManContext context)
        {
            context.Database.EnsureCreated();

            SeedRoles(roleManager).Wait();
            SeedUsers(userManager, context).Wait();
            if (SeedServiceTypes(context))
            {
                SeedServices(context);
            }
        }

        public async static Task SeedRoles( RoleManager<IdentityRole> roleManager)
        {
            // Check if the admin role exist first, only add if it does not exist in the database
            if (!roleManager.RoleExistsAsync(adminRole).Result)
            {
                var role = new IdentityRole(adminRole);
                await roleManager.CreateAsync(role);
            }

            // Check if the member role exist first, only add if it does not exist in the database
            if (!roleManager.RoleExistsAsync(memberRole).Result)
            {
                var role = new IdentityRole(memberRole);
                await roleManager.CreateAsync(role);
            }
        }

        public async static Task SeedUsers(UserManager<ApplicationUser> userManager, RiverManContext context)
        {
            // Add an Admin user
            // Check if the user exist first, only add if the user does not yet exist
            if (userManager.FindByEmailAsync("admin@riverman.com").Result == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "NicksAdmin",
                    Email = "admin@riverman.com",
                    LastLogin = DateTime.Now
                };

                var result = await userManager.CreateAsync(user, "My_password1");

                // If the admin user was successfully added, add to the admin role
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, adminRole);
                }
            }

            // Add yourself as a Customer
            // Check if the user exist first, only add if the user does not yet exist
            if (userManager.FindByEmailAsync("user@riverman.com").Result == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "NicksUser",
                    Email = "user@riverman.com",
                    PhoneNumber = "01639123456",
                    LastLogin = DateTime.Now
                };

                var result = await userManager.CreateAsync(user, "My_password1");

                // If the customer user was successfully added, add as a customer and to the member role
                if (result.Succeeded)
                {
                    // Add the customer to the member role
                    await userManager.AddToRoleAsync(user, memberRole);

                    // Save the changes to the database
                    await context.SaveChangesAsync();
                }
            }

        }

        public static bool SeedServiceTypes(RiverManContext context)
        {
            var type = context.ServiceTypes.FirstOrDefault(c => c.Name == "Streaming");
            if (type != null)
            {
                ServiceType mediaStreaming = new ServiceType { Name = "Streaming" };
                context.ServiceTypes.Add(mediaStreaming);
                bool success = context.SaveChanges() > 0;
                if (success)
                    streamingServiceType = mediaStreaming;
                return success;
            }
            return false;
        }

        public static bool SeedServices(RiverManContext context)
        {
            var type = context.SubscriptionServices.FirstOrDefault(c => c.ServiceName == "Netflix");
            if (type != null)
            {
                SubscriptionService netflix = new SubscriptionService
                {
                    ServiceName = "Netflix",
                    ServiceType = streamingServiceType
                };
                context.SubscriptionServices.Add(netflix);
                return context.SaveChanges() > 0;
            }
            return false;
        }
    }
}

using _AbsoPickUp.IServices;
using _AbsoPickUp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace _AbsoPickUp.Data
{
    public static class ServiceDescription
    {
        public static IHost MigrateDbContext<TContext>(this IHost host) where TContext : DbContext
        {
            // Create a scope to get scoped services.
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    var userManager = services.GetRequiredService<UserManager<UserDetails>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var netcoreService = services.GetRequiredService<IFunctionalService>();
                    //DbInitializer.Initialize(context, userManager, roleManager, netcoreService).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<TContext>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            return host;
        }
    }
}

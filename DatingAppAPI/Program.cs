using System;
using System.Threading.Tasks;
using DatingAppAPI.Data;
using DatingAppAPI.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DatingAppAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            //defining scope of the service that we will be using.
            using var scope = host.Services.CreateScope();
            //define service provider.
            var services = scope.ServiceProvider;

            try
            {
                //create datacontext service.
                var context = services.GetRequiredService<DataContext>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
                //it will automatically migrate the latest migration when we restart our application.
                await context.Database.MigrateAsync();
                //seed data
                await Seed.SeedUsers(userManager,roleManager);
            }
            catch (Exception ex)
            {
                //get logger service.
                var logger = services.GetRequiredService<ILogger<Program>>();
                //log error
                logger.LogError(ex, "An error occured during migration");
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

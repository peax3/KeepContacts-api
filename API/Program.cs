using Entities.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
   public class Program
   {
      public static async Task Main(string[] args)
      {
         var host = CreateHostBuilder(args).Build();

         using var scope = host.Services.CreateScope();
         var services = scope.ServiceProvider;

         try
         {
            var context = services.GetService<DataContext>();
            var userManager = services.GetService<UserManager<AppUser>>();
            await context.Database.MigrateAsync();
            await Seed.SeedData(context, userManager);
         }
         catch (Exception ex)
         {
            var logger = services.GetService<ILogger<Program>>();
            logger.LogError(ex, "An Error occured during migration");
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
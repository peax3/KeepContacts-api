using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace API.Extensions
{
   public static class ServicesExtensions
   {
      public static void AddSqliteConnection(this IServiceCollection services, IConfiguration configuration)
      {
         services.AddDbContext<DataContext>(
            opts => opts.UseSqlite(configuration.GetConnectionString("DefaultConnection"))
            );
      }
   }
}
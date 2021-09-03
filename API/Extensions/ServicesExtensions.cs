using Entities.Models;
using Infrastructure.Avatar;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using System.Text;

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

		public static void AddIdentityService(this IServiceCollection services)
		{
			services.AddIdentityCore<AppUser>(options =>
			{
				options.Password.RequiredLength = 8;
				options.Password.RequireDigit = true;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireLowercase = false;
				options.User.RequireUniqueEmail = true;
				options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-. ";
			})
			 .AddEntityFrameworkStores<DataContext>();
		}

		public static void AddJwtTokenAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("TokenKey")));

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = key,
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});
		}

		public static void AddAuthorizationPolicy(this IServiceCollection services)
		{
			services.AddAuthorization(options =>
			{
				options.AddPolicy("IsOwnerRequirement", policy =>
				{
					policy.AddRequirements(new IsOwnerRequirement());
				});
			});

			services.AddScoped<IAuthorizationHandler, IsOwnerRequirementHandler>();
		}

		public static void ConfigureCloudinaryAccount(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<CloudinaryAccount>(configuration.GetSection("Cloudinary"));
		}
	}
}
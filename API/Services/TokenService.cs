using Entities.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Services
{
   public class TokenService
   {
      private readonly IConfiguration _config;

      public TokenService(IConfiguration config)
      {
         this._config = config;
      }

      public string CreateToken(AppUser user)
      {
         var claims = new List<Claim>
         {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email)
         };

         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("TokenKey")));

         var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

         var tokenDescriptor = new SecurityTokenDescriptor
         {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = credentials
         };

         var tokenHandler = new JwtSecurityTokenHandler();

         var token = tokenHandler.CreateToken(tokenDescriptor);

         return tokenHandler.WriteToken(token);
      }
   }
}
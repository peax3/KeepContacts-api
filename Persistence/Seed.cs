using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
   public class Seed
   {
      public static async Task SeedData(DataContext dataContext, UserManager<AppUser> userManager)
      {
         if (dataContext.Contacts.Any() || userManager.Users.Any())
         {
            return;
         }

         var users = new List<AppUser>
         {
            new AppUser
            {
               UserName = "John Doe",
               Email = "Johndoe@test.com"
            },
            new AppUser
            {
               UserName = "Tom Jerry",
               Email = "Tomjerry@test.com"
            }
         };

         foreach (var user in users)
         {
            await userManager.CreateAsync(user, "Pa$$w0rd");
         }

         var usersFromDb = await userManager.Users.ToListAsync();

         var contacts = new List<Contact>
         {
            new Contact
            {
               Name="Justin Beiber",
               Email="justinbeiber@test.com",
               PhoneNumber="0023527650",
               Owner = usersFromDb[0]
            },
            new Contact
            {
               Name="Lady Gaga",
               Email="ladygaga@test.com",
               PhoneNumber="0079643394",
               Owner = usersFromDb[1]
            },
            new Contact
            {
               Name="Katty Perry",
               Email="Justin@gmail.com",
               PhoneNumber="00679432800",
               Owner = usersFromDb[1]
            }
         };

         await dataContext.Contacts.AddRangeAsync(contacts);
         await dataContext.SaveChangesAsync();
      }
   }
}
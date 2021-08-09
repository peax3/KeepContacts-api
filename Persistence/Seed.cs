using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
   public class Seed
   {
      public static async Task SeedData(DataContext dataContext)
      {
         if (dataContext.Contacts.Any()) return;

         var contacts = new List<Contact>
         {
            new Contact
            {
               Name="Justin Beiber",
               Email="justinbeiber@test.com",
               PhoneNumber="0023527650"
            },
            new Contact
            {
               Name="Lady Gaga",
               Email="ladygaga@test.com",
               PhoneNumber="0079643394"
            },
            new Contact
            {
               Name="Katty Perry",
               Email="Justin@gmail.com",
               PhoneNumber="00679432800"
            }
         };

         await dataContext.Contacts.AddRangeAsync(contacts);
         await dataContext.SaveChangesAsync();
      }
   }
}
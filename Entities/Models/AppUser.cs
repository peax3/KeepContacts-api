using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
   public class AppUser : IdentityUser
   {
      // Relationship
      public ICollection<Contact> Contacts { get; set; }
   }
}
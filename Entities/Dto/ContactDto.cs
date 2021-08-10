using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dto
{
   public class ContactDto
   {
      [Required(ErrorMessage = "Contact Name is required")]
      public string Name { get; set; }

      [Phone(ErrorMessage = "Enter a valid Phone number")]
      public string PhoneNumber { get; set; }

      [EmailAddress(ErrorMessage = "Enter a valid Email address")]
      public string Email { get; set; }

      [MaxLength(100, ErrorMessage = "Address cannot be more than 100 characters")]
      public string Address { get; set; }
   }
}
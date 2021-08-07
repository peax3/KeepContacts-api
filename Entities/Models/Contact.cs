﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
   public class Contact
   {
      public Guid ContactId { get; set; }
      public string Name { get; set; }
      public string PhoneNumber { get; set; }
      public string Address { get; set; }
   }
}
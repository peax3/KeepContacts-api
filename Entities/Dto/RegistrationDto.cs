using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dto
{
	public class RegistrationDto
	{
		[Required(ErrorMessage = "Name is required")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Email field is required")]
		[EmailAddress(ErrorMessage = "Please enter a valid email")]
		public string Email { get; set; }

		[Required]
		[MinLength(8, ErrorMessage = "Password must be more than 8 eight characters")]
		public string Password { get; set; }
	}
}
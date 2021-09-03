using System;

namespace Entities.Models
{
	public class Contact
	{
		public Guid ContactId { get; set; }
		public string Name { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public string Address { get; set; }

		// relationship
		public AppUser Owner { get; set; }

		public Avatar Avatar { get; set; }
	}
}
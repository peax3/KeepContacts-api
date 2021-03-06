using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Interfaces
{
	public interface IContactRepository
	{
		Task<IEnumerable<Contact>> GetContacts(string ownerId, bool trackChanges);

		Task<Contact> GetContact(Guid id, bool trackChanges);

		Task DeleteContact(Contact contactToDelete);

		Task<bool> CreateContact(Contact contactToSave);

		Task<bool> UpdateContact(Contact contactToUpdate);
	}
}
using Entities.Interfaces;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence
{
	public class ContactRepository : IContactRepository
	{
		private readonly DataContext _dataContext;

		public ContactRepository(DataContext dataContext)
		{
			_dataContext = dataContext;
		}

		public async Task<bool> CreateContact(Contact contact)
		{
			await _dataContext.Contacts.AddAsync(contact);
			return await _dataContext.SaveChangesAsync() > 0;
		}

		public async Task DeleteContact(Contact contactToDelete)
		{
			_dataContext.Contacts.Remove(contactToDelete);
			await _dataContext.SaveChangesAsync();
		}

		public async Task<Contact> GetContact(Guid id, bool trackChanges)
		{
			var contact =
				trackChanges ?
				await _dataContext.Contacts.AsTracking()
	 .Include(c => c.Owner)
	 .Include(c => c.Avatar)
	 .FirstOrDefaultAsync(contact => contact.ContactId == id) :
				await _dataContext.Contacts.AsNoTracking()
	 .Include(c => c.Owner)
	 .Include(c => c.Avatar)
	 .FirstOrDefaultAsync(contact => contact.ContactId == id);

			return contact;
		}

		public async Task<IEnumerable<Contact>> GetContacts(string ownerId, bool trackChanges)
		{
			return trackChanges ?
				await _dataContext.Contacts.AsTracking()
	 .Include(c => c.Avatar)
	 .Include(c => c.Owner)
	 .Where(c => c.Owner.Id == ownerId).ToListAsync() :
				await _dataContext.Contacts.AsNoTracking()
	 .Include(c => c.Avatar)
	 .Include(c => c.Owner)
	 .Where(c => c.Owner.Id == ownerId)
	 .ToListAsync();
		}

		public async Task<bool> UpdateContact(Contact contactToUpdate)
		{
			return await _dataContext.SaveChangesAsync() > 0;
		}
	}
}
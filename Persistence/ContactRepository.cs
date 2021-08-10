using Entities.Interfaces;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

      public async Task<Contact> GetContact(Guid id)
      {
         var contact = await _dataContext.Contacts.AsNoTracking().FirstOrDefaultAsync(contact => contact.ContactId == id);
         return contact;
      }

      public async Task<IEnumerable<Contact>> GetContacts()
      {
         return await _dataContext.Contacts.ToListAsync();
      }

      public async Task<bool> UpdateContact(Contact contactToUpdate)
      {
         _dataContext.Entry<Contact>(contactToUpdate).State = EntityState.Modified;
         return await _dataContext.SaveChangesAsync() > 0;
      }
   }
}
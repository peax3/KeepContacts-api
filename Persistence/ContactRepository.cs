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

      public async Task<Contact> GetContact(Guid id)
      {
         var contact = await _dataContext.Contacts.FindAsync(id);
         return contact;
      }

      public async Task<IEnumerable<Contact>> GetContacts()
      {
         return await _dataContext.Contacts.ToListAsync();
      }
   }
}
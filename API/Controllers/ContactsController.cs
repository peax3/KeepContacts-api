using Entities.Interfaces;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class ContactsController : ControllerBase
   {
      private readonly IContactRepository _contactRepository;

      public ContactsController(IContactRepository contactRepository)
      {
         _contactRepository = contactRepository;
      }

      [HttpGet]
      public async Task<ActionResult<IEnumerable<Contact>>> GetAllContacts()
      {
         var contacts = await _contactRepository.GetContacts();
         return Ok(contacts);
      }

      [HttpGet("{id}")]
      public async Task<ActionResult<Contact>> GetContact(Guid id)
      {
         var contact = await _contactRepository.GetContact(id);
         if (contact == null) return NotFound();
         return Ok(contact);
      }
   }
}
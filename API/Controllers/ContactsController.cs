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

      [HttpPost]
      public async Task<IActionResult> CreateContact(Contact contact)
      {
         if (contact == null) return BadRequest();

         var isSuccessful = await _contactRepository.CreateContact(contact);
         if (!isSuccessful) return BadRequest("Failed to save contact");

         return new StatusCodeResult(201);
      }

      [HttpPut("{id}")]
      public async Task<IActionResult> UpdateContact(Guid id, [FromBody] Contact contactFromRequest)
      {
         var contact = await _contactRepository.GetContact(id);
         if (contact == null) return NotFound();

         var ContactToUpdate = new Contact
         {
            ContactId = contact.ContactId,
            Name = contactFromRequest.Name ?? contact.Name,
            Email = contactFromRequest.Email ?? contact.Name,
            PhoneNumber = contactFromRequest.PhoneNumber ?? contact.PhoneNumber,
            Address = contactFromRequest.Address ?? contact.PhoneNumber
         };

         var isSuccessful = await _contactRepository.UpdateContact(ContactToUpdate);
         if (!isSuccessful) return BadRequest("Failed to Update contact");

         return Ok();
      }

      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteContact(Guid id)
      {
         var contactToDelete = await _contactRepository.GetContact(id);
         if (contactToDelete == null)
            return NotFound();

         await _contactRepository.DeleteContact(contactToDelete);
         return Ok();
      }
   }
}
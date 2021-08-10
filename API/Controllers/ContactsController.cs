using AutoMapper;
using Entities.Dto;
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
      private readonly IMapper _mapper;

      public ContactsController(IContactRepository contactRepository, IMapper mapper)
      {
         _contactRepository = contactRepository;
         this._mapper = mapper;
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
      public async Task<IActionResult> CreateContact([FromBody] ContactDto contactFromRequest)
      {
         if (contactFromRequest == null) return BadRequest();

         var contact = _mapper.Map<Contact>(contactFromRequest);

         var isSuccessful = await _contactRepository.CreateContact(contact);
         if (!isSuccessful) return BadRequest("Failed to save contact");

         return new StatusCodeResult(201);
      }

      [HttpPut("{id}")]
      public async Task<IActionResult> UpdateContact(Guid id, [FromBody] ContactDto contactFromRequest)
      {
         var contactToUpdate = await _contactRepository.GetContact(id);
         if (contactToUpdate == null) return NotFound();

         _mapper.Map(contactFromRequest, contactToUpdate);

         var isSuccessful = await _contactRepository.UpdateContact(contactToUpdate);
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
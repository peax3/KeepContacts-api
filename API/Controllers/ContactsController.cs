using AutoMapper;
using Entities.Dto;
using Entities.Interfaces;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class ContactsController : ControllerBase
	{
		private readonly IContactRepository _contactRepository;
		private readonly IMapper _mapper;
		private readonly IAuthorizationService _authService;
		private readonly UserManager<AppUser> _userManager;

		public ContactsController(IContactRepository contactRepository, IMapper mapper, IAuthorizationService authService, UserManager<AppUser> userManager)
		{
			_contactRepository = contactRepository;
			this._mapper = mapper;
			this._authService = authService;
			this._userManager = userManager;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Contact>>> GetAllContacts()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var contacts = await _contactRepository.GetContacts(userId, false);

			var contactReponeDtos = _mapper.Map<IEnumerable<ContactResponseDto>>(contacts);
			return Ok(contactReponeDtos);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Contact>> GetContact(Guid id)
		{
			var contact = await _contactRepository.GetContact(id, false);

			if (contact == null) return NotFound();

			var authResult = await _authService.AuthorizeAsync(User, contact, "IsOwnerRequirement");
			if (!authResult.Succeeded) return new ForbidResult();

			var contactResponstDto = _mapper.Map<ContactResponseDto>(contact);
			return Ok(contactResponstDto);
		}

		[HttpPost]
		public async Task<IActionResult> CreateContact([FromBody] ContactDto contactFromRequest)
		{
			if (contactFromRequest == null) return BadRequest();

			var contact = _mapper.Map<Contact>(contactFromRequest);

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == userId);

			contact.Owner = user;

			var isSuccessful = await _contactRepository.CreateContact(contact);
			if (!isSuccessful) return BadRequest("Failed to save contact");

			return new StatusCodeResult(201);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateContact(Guid id, [FromBody] ContactDto contactFromRequest)
		{
			var contactToUpdate = await _contactRepository.GetContact(id, true);
			if (contactToUpdate == null) return NotFound();

			var authResult = await _authService.AuthorizeAsync(User, contactToUpdate, "IsOwnerRequirement");
			if (!authResult.Succeeded) return new ForbidResult();

			_mapper.Map(contactFromRequest, contactToUpdate);

			var isSuccessful = await _contactRepository.UpdateContact(contactToUpdate);
			if (!isSuccessful) return BadRequest("Failed to Update contact");
			return Ok();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteContact(Guid id)
		{
			var contactToDelete = await _contactRepository.GetContact(id, true);
			if (contactToDelete == null)
				return NotFound();

			var authResult = await _authService.AuthorizeAsync(User, contactToDelete, "IsOwnerRequirement");
			if (!authResult.Succeeded) return new ForbidResult();

			await _contactRepository.DeleteContact(contactToDelete);
			return Ok();
		}
	}
}
using Entities.Interfaces;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
	[Authorize]
	[Route("contacts/{contactId}/avatar")]
	[ApiController]
	public class AvatarController : ControllerBase
	{
		private readonly IAuthorizationService _authService;
		private readonly IContactRepository _contactRepository;
		private readonly IAvatarAccessor _avatarAccessor;
		private readonly DataContext _dataContext;

		public AvatarController(IAuthorizationService authService, IContactRepository contactRepository, IAvatarAccessor avatarAccessor, DataContext dataContext)
		{
			this._authService = authService;
			this._contactRepository = contactRepository;
			this._avatarAccessor = avatarAccessor;
			this._dataContext = dataContext;
		}

		[HttpPost]
		public async Task<IActionResult> AddAvatar(Guid contactId, [FromForm] IFormFile file)
		{
			if (file == null)
			{
				return BadRequest("Upload Avatar");
			}

			var contact = await _contactRepository.GetContact(contactId, true);
			if (contact == null) return NotFound();
			var authResult = await _authService.AuthorizeAsync(User, contact, "IsOwnerRequirement");
			if (!authResult.Succeeded) return new ForbidResult();

			var avatarResult = await _avatarAccessor.AddPhoto(file);

			contact.Avatar = new Avatar
			{
				Id = avatarResult.PublicId,
				Url = avatarResult.Url
			};

			var isSuccessful = await _contactRepository.UpdateContact(contact);
			if (!isSuccessful) return BadRequest("Failed to upload avatar");

			return Ok(avatarResult);
		}

		[HttpDelete("{avatarId}")]
		public async Task<IActionResult> DeleteAvatar(Guid contactId, string avatarId)
		{
			var contact = await _contactRepository.GetContact(contactId, true);
			if (contact == null) return NotFound();
			var authResult = await _authService.AuthorizeAsync(User, contact, "IsOwnerRequirement");

			if (!authResult.Succeeded) return new ForbidResult();

			await _avatarAccessor.DeletePhoto(avatarId);

			var avatarToDelete = await _dataContext.Avatars.FirstOrDefaultAsync(a => a.Id == contact.Avatar.Id);
			_dataContext.Avatars.Remove(avatarToDelete);
			contact.Avatar = null;

			var isSuccessful = await _dataContext.SaveChangesAsync() > 0;
			if (!isSuccessful) return BadRequest("Failed to delete avatar");

			return Ok();
		}
	}
}
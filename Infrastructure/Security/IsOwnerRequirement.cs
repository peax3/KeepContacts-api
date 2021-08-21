using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security
{
	public class IsOwnerRequirement : IAuthorizationRequirement
	{
	}

	public class IsOwnerRequirementHandler : AuthorizationHandler<IsOwnerRequirement, Contact>
	{
		private readonly UserManager<AppUser> _userManager;

		public IsOwnerRequirementHandler(UserManager<AppUser> userManager)
		{
			this._userManager = userManager;
		}

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsOwnerRequirement requirement, Contact contact)
		{
			var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null) return;

			var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == userId);

			if (user == null) return;

			if (contact.Owner.Id == user.Id) context.Succeed(requirement);
		}
	}
}
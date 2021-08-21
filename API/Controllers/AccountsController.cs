using API.Services;
using Entities.Dto;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AccountsController : ControllerBase
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly TokenService _tokenService;

		public AccountsController(UserManager<AppUser> userManager, TokenService tokenService)
		{
			this._userManager = userManager;
			this._tokenService = tokenService;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
		{
			var user = await _userManager.Users.FirstOrDefaultAsync(user => user.NormalizedEmail == loginDto.Email.ToUpper());

			if (user == null) return Unauthorized();

			var isUserValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

			if (isUserValid)
			{
				return Ok(CreateTokenObject(user));
			}
			else
			{
				return Unauthorized();
			}
		}

		[HttpPost("register")]
		public async Task<IActionResult> RegisterUser([FromBody] RegistrationDto registrationDto)
		{
			var user = new AppUser
			{
				Email = registrationDto.Email,
				UserName = registrationDto.Name
			};

			var registrationResult = await _userManager.CreateAsync(user, registrationDto.Password);

			if (!registrationResult.Succeeded)
			{
				foreach (var error in registrationResult.Errors)
				{
					ModelState.TryAddModelError(error.Code, error.Description);
				}

				return ValidationProblem();
			}

			return Ok(CreateTokenObject(user));
		}

		private object CreateTokenObject(AppUser user)
		{
			return new { Token = _tokenService.CreateToken(user) };
		}
	}
}
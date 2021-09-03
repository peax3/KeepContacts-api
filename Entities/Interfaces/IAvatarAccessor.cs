using Entities.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Entities.Interfaces
{
	public interface IAvatarAccessor
	{
		Task<AvatarResult> AddPhoto(IFormFile file);

		Task DeletePhoto(string publicId);
	}
}
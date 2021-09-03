using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Entities.Interfaces;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Avatar
{
	public class AvatarAccessor : IAvatarAccessor
	{
		private readonly Cloudinary _cloudinary;

		public AvatarAccessor(IOptions<CloudinaryAccount> config)
		{
			var account = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);

			_cloudinary = new Cloudinary(account);
		}

		public async Task<AvatarResult> AddPhoto(IFormFile file)
		{
			if (file.Length > 0)
			{
				using var stream = file.OpenReadStream();
				var uploadParams = new ImageUploadParams
				{
					File = new FileDescription(file.FileName, stream),
					Transformation = new Transformation().Height(500).Width(500)
				};

				var uploadResult = await _cloudinary.UploadAsync(uploadParams);
				if (uploadResult.Error != null)
				{
					throw new Exception(uploadResult.Error.Message);
				}

				return new AvatarResult
				{
					PublicId = uploadResult.PublicId,
					Url = uploadResult.SecureUrl.ToString(),
				};
			}

			return null;
		}

		public async Task DeletePhoto(string publicId)
		{
			var deleteParams = new DeletionParams(publicId);
			await _cloudinary.DestroyAsync(deleteParams);
		}
	}
}
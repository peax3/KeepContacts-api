using AutoMapper;
using Entities.Dto;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<ContactDto, Contact>();
			CreateMap<Contact, ContactResponseDto>();
		}
	}
}
using System;
using AutoMapper;
using GrowingStrongAPI.Models;

namespace GrowingStrongAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegistrationDetails, User>();
        }
    }
}

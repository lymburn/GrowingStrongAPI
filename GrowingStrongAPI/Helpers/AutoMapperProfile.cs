using System;
using AutoMapper;
using GrowingStrongAPI.Models;
using GrowingStrongAPI.Entities;

namespace GrowingStrongAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegistrationModel, User>();
            CreateMap<RegistrationModel, UserDto>();
            CreateMap<RegistrationModel, UserProfileDto>();
            CreateMap<RegistrationModel, UserTargetsDto>();
            CreateMap<User, UserDto>().ForMember(dest => dest.Targets, opt => opt.MapFrom(src => src.UserTargets))
                                      .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.UserProfile));
            CreateMap<FoodEntry, FoodEntryDto>();
            CreateMap<Food, FoodDto>();
            CreateMap<Serving, ServingDto>();
            CreateMap<UserProfile, UserProfileDto>();
            CreateMap<UserTargets, UserTargetsDto>();
        }
    }
}

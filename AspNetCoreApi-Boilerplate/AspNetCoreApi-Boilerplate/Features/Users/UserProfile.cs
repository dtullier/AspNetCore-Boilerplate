using AspNetCoreApi_Boilerplate.Controllers;
using AspNetCoreApi_Boilerplate.Data.Entities;
using AutoMapper;
using System.Linq;

namespace AspNetCoreApi_Boilerplate.Features.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, GetUserDto>();

            CreateMap<CreateUserRequest, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        }
    }
}

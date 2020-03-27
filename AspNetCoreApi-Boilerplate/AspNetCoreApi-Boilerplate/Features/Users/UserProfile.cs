using AspNetCoreApi_Boilerplate.Controllers;
using AspNetCoreApi_Boilerplate.Data.Entities;
using AspNetCoreApi_Boilerplate.Dtos;
using AutoMapper;

namespace AspNetCoreApi_Boilerplate.Features.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserRequest, User>();

            CreateMap<User, GetUserDto>();
        }
    }
}

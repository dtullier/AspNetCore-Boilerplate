using AspNetCoreApi_Boilerplate.Data.Entities;
using System.Collections.Generic;

namespace AspNetCoreApi_Boilerplate.Dtos
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class GetUserAuthenticationDto
    {
        public int Id { get; set; }
        public string Token { get; set; }
        //public string RefreshToken { get; set; }
    }
}

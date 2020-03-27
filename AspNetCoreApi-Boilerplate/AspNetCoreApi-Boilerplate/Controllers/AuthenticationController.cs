using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreApi_Boilerplate.Data;
using AspNetCoreApi_Boilerplate.Data.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AspNetCoreApi_Boilerplate.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;

        public AuthenticationController(
            DataContext context,
            IMapper mapper,
            UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            RoleManager<Role> roleManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthenticateUserRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok(new UserDto
            {
                Email = user.Email,
                Token = GenerateJwtToken(user.Id.ToString())
            });
        }

        [AllowAnonymous]
        [HttpPost("create-user")]
        public async Task<GetUserDto> CreateUser(CreateUserRequest request)
        {
            var user = _mapper.Map<User>(request);
            await _userManager.CreateAsync(user, request.Password);
            //await _userManager.AddToRoleAsync(user, "Admin");

            return _mapper.Map<GetUserDto>(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("get-user")]
        public async Task<GetUserDto> GetUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return _mapper.Map<GetUserDto>(user);
        }

        public static string GenerateJwtToken(string entityId)
        {
            var key = Encoding.ASCII.GetBytes("secretthisisandimtestingbecauseithinkitneedstobeprettylong");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Sid, entityId),
                    new Claim(ClaimTypes.Role, "Admin")
                }),

                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }

    public class UserDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class AuthenticateUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class CreateUserRequest
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }

    public class GetUserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
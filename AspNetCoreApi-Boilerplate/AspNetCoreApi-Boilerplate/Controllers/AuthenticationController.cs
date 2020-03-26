using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreApi_Boilerplate.Data.Entities;
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
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;

        public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthenticateUserRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                const string passwordForEveryone = "Password123!";
                var userToAdd = new User { UserName = request.Username };
                await _userManager.CreateAsync(userToAdd, passwordForEveryone);
                await _roleManager.CreateAsync(new Role { Name = "Admin" });
                await _userManager.AddToRoleAsync(userToAdd, "Admin");
                user = await _userManager.FindByNameAsync(request.Username);
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok(new UserDto
            {
                Username = user.UserName,
                Token = GenerateJwtToken(user.Id.ToString())
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("test")]
        public async Task<IActionResult> test()
        {
            return Ok(new UserDto
            {
                Username = "test"
            });
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

    internal class UserDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
    }

    public class AuthenticateUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
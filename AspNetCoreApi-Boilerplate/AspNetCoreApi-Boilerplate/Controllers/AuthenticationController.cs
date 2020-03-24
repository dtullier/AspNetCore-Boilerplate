using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreApi_Boilerplate.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreApi_Boilerplate.Controllers
{
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
        [HttpPost("[controller]/authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateUserRequest request)
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
            await _signInManager.SignInAsync(user, false, "Password");
            return Ok(new UserDto
            {
                Username = user.UserName
            });
        }
    }

    internal class UserDto
    {
        public string Username { get; set; }
    }

    public class AuthenticateUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
using System.Net;
using System.Threading.Tasks;
using AspNetCoreApi_Boilerplate.Common.Constants;
using AspNetCoreApi_Boilerplate.Common.Responses;
using AspNetCoreApi_Boilerplate.Dtos;
using AspNetCoreApi_Boilerplate.Features.Users;
using AspNetCoreApi_Boilerplate.MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreApi_Boilerplate.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : MediatorControllerBase
    {
        private readonly IMediatorService _mediatorService;

        public UsersController(
            IMediatorService mediatorService)
        {
            _mediatorService = mediatorService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        [ProducesResponseType(typeof(Response<GetUserAuthenticationDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Authenticate(AuthenticateUserRequest request)
        {
            var result = await _mediatorService.Send(request);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(Response<GetUserDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {
            var result = await _mediatorService.Send(request);
            return Ok(result);
        }

        [Authorize(Roles = EntityConstants.Role.Admin)]
        [HttpGet("get-all")]
        [ProducesResponseType(typeof(Response<GetUserDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllUser()
        {
            var request = new GetAllUsersRequest();
            var result = await _mediatorService.Send(request);
            return Ok(result);
        }
    }
}
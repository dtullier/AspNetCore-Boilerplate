using AspNetCoreApi_Boilerplate.Common.Responses;
using AspNetCoreApi_Boilerplate.MediatR.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AspNetCoreApi_Boilerplate.MediatR
{
    public abstract class MediatorControllerBase : ControllerBase
    {
        public IActionResult Ok<TResponse>(Response<TResponse> response)
            where TResponse : class, new()
        {
            if (!response.IsValid) return BadRequest(response.ToEmptyData());
            return base.Ok(response);
        }

        public IActionResult Created<TResponse>(string uri, Response<TResponse> response)
            where TResponse : class, new()
        {
            if (!response.IsValid) return BadRequest(response.ToEmptyData());
            return base.Created(uri, response);
        }

        public IActionResult BadRequest<TResponse>(Response<TResponse> response)
            where TResponse : class, new()
        {
            return base.BadRequest(response);
        }

        protected int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.Sid).Value);
        }
    }
}

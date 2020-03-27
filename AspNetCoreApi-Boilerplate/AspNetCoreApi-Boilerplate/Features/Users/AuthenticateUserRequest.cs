using AspNetCoreApi_Boilerplate.Data;
using AspNetCoreApi_Boilerplate.Data.Entities;
using AspNetCoreApi_Boilerplate.Dtos;
using AspNetCoreApi_Boilerplate.Infrastructure;
using AspNetCoreApi_Boilerplate.Infrastructure.Security;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreApi_Boilerplate.Features.Users
{
    public class AuthenticateUserRequest : IRequest<GetUserAuthenticationDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthenticateUserRequestHandler :
        IRequestHandler<AuthenticateUserRequest, GetUserAuthenticationDto>
    {
        private readonly DataContext _context;
        private readonly AppSettings _appSettings;

        public AuthenticateUserRequestHandler(
            DataContext context,
            IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public async Task<GetUserAuthenticationDto> Handle(
            AuthenticateUserRequest request,
            CancellationToken cancellationToken)
        {
            var user = _context.Set<User>().First(x => x.Email == request.Email);
            var roles = user.Roles.Select(x => x.Role.Name);

            var userToReturn = new GetUserAuthenticationDto
            {
                Id = user.Id,
                Token = Authentication.GenerateJwtToken(
                            _appSettings,
                            user.Id.ToString(),
                            roles,
                            DateTime.UtcNow.AddDays(7))
                //Need to change the expiration date when pushed to production
            };

            return userToReturn;
        }
    }

    public class AuthenticateUserRequestValidator :
        AbstractValidator<AuthenticateUserRequest>
    {
        private readonly DataContext _context;

        private bool _emailPassed = true;
        private bool _passwordPassed = true;

        public AuthenticateUserRequestValidator(
            DataContext context)
        {
            _context = context;

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(256)
                .OnFailure(x => _emailPassed = false);

            RuleFor(x => x.Password)
                .NotEmpty()
                .OnFailure(x => _passwordPassed = false);

            RuleFor(x => x)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Must(ExistInDatabase)
                .HideFromPreValidation()
                .When(x => _emailPassed && _passwordPassed)
                .WithMessage("incorrect")
                .Must(BeCorrectEmailAndPassword)
                .HideFromPreValidation()
                .When(x => _emailPassed && _passwordPassed)
                .WithMessage("incorrect");
        }

        private bool ExistInDatabase(AuthenticateUserRequest arg)
        {
            return _context.Set<User>().Any(x => x.Email == arg.Email);
        }

        private bool BeCorrectEmailAndPassword(AuthenticateUserRequest arg)
        {
            var user = _context.Set<User>().Single(x => x.Email == arg.Email);
            var passwordHash = new PasswordHash(user.PasswordSalt, user.PasswordHash);
            return passwordHash.Verify(arg.Password);
        }
    }
}

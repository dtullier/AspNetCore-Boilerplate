using AspNetCoreApi_Boilerplate.Common.ErrorMessages;
using AspNetCoreApi_Boilerplate.Data;
using AspNetCoreApi_Boilerplate.Data.Entities;
using AspNetCoreApi_Boilerplate.Dtos;
using AspNetCoreApi_Boilerplate.Infrastructure.Security;
using AutoMapper;
using FluentValidation;
using MediatR;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreApi_Boilerplate.Features.Users
{
    public class CreateUserRequest : IRequest<GetUserDto>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }

    public class CreateUserRequestHandler :
        IRequestHandler<CreateUserRequest, GetUserDto>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CreateUserRequestHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetUserDto> Handle(
            CreateUserRequest request,
            CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);

            var hasher = new PasswordHash(request.Password);
            user.PasswordHash = hasher.Hash;
            user.PasswordSalt = hasher.Salt;

            await _context.Set<User>().AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<GetUserDto>(user);
        }
    }

    public class CreateUserRequestValidator :
        AbstractValidator<CreateUserRequest>
    {
        private readonly DataContext _context;

        public CreateUserRequestValidator(DataContext context)
        {
            _context = context;

            RuleFor(x => x.FirstName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(x => x.LastName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(256)
                .DependentRules(() =>
                {
                    RuleFor(x => x)
                        .Must(BeUniqueEmail)
                        .WithMessage(ErrorMessages.User.EmailAlreadyExists)
                        .OverridePropertyName("Email");
                });

            RuleFor(x => x.PhoneNumber)
                .Must(BeEmptyOrValidPhoneNumber)
                .WithMessage(ErrorMessages.User.NotValidPhoneNumber);

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .DependentRules(() =>
                {
                    RuleFor(x => x.Password)
                        .MinimumLength(8)
                        .WithMessage(ErrorMessages.User.PasswordIsTooShort)
                        .Must(ContainUpperCase)
                        .WithMessage(ErrorMessages.User.PasswordDoesNotHaveUpper)
                        .Must(ContainDigit)
                        .WithMessage(ErrorMessages.User.PasswordDoesNotHaveDigit)
                        .Must(ContainSymbol)
                        .WithMessage(ErrorMessages.User.PasswordDoesNotHaveSymbol);
                });
        }

        private bool BeUniqueEmail(CreateUserRequest request)
        {
            return !_context.Set<User>().Any(x => x.Email == request.Email);
        }

        private bool BeEmptyOrValidPhoneNumber(string number)
        {
            if (string.IsNullOrEmpty(number))
                return true;

            var cleaned = Regex.Replace(number, @"[^0-9]+", "");
            if (cleaned.Length == 10)
                return true;
            else
                return false;
        }

        private bool ContainUpperCase(string password)
        {
            return password.Any(char.IsUpper);
        }

        private bool ContainDigit(string password)
        {
            return password.Any(char.IsDigit);
        }

        private bool ContainSymbol(string password)
        {
            return password.Any(char.IsSymbol) || password.Any(char.IsPunctuation);
        }
    }
}

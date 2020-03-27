using AspNetCoreApi_Boilerplate.Data;
using AspNetCoreApi_Boilerplate.Data.Entities;
using AspNetCoreApi_Boilerplate.Dtos;
using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreApi_Boilerplate.Features.Users
{

    public class GetAllUsersRequest : IRequest<List<GetUserDto>>
    {
    }

    public class GetAllUsersRequestHandler :
        IRequestHandler<GetAllUsersRequest, List<GetUserDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public GetAllUsersRequestHandler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GetUserDto>> Handle(
            GetAllUsersRequest request,
            CancellationToken cancellationToken)
        {
            return await _context
                .Set<User>()
                .ProjectTo<GetUserDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }

    public class GetAllUsersRequestValidator :
        AbstractValidator<GetAllUsersRequest>
    {
        public GetAllUsersRequestValidator()
        {

        }
    }
}

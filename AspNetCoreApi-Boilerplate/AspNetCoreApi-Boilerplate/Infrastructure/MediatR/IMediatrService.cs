using AspNetCoreApi_Boilerplate.Common.Responses;
using MediatR;
using System.Threading.Tasks;

namespace AspNetCoreApi_Boilerplate.MediatR
{
    public interface IMediatorService
    {
        Task<Response<TResponse>> Send<TResponse>(IRequest<TResponse> request)
            where TResponse : class, new();
    }
}

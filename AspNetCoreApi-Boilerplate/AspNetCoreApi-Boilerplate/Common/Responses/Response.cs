using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreApi_Boilerplate.Common.Responses
{
    public class Response
    {
        public bool IsValid => !ErrorMessages.Any();
        public List<ApiErrorMessage> ErrorMessages { get; set; } = new List<ApiErrorMessage>();
    }

    public class Response<T> : Response
    {
        public T Data { get; set; }
    }

    public class EmptyResponse
    {

    }
}

using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreApi_Boilerplate.Common.Responses
{
    public class Response
    {
        public bool IsValid => !ErrorMessages.Any();
        public List<ErrorMessage> ErrorMessages { get; set; } = new List<ErrorMessage>();
    }

    public class Response<T> : Response
    {
        public T Data { get; set; }
    }

    public class EmptyResponse
    {

    }
}

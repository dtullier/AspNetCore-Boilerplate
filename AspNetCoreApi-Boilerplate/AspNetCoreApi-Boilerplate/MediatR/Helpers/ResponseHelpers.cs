using AspNetCoreApi_Boilerplate.Common.Responses;

namespace AspNetCoreApi_Boilerplate.MediatR.Helpers
{
    public static class ResponseHelpers
    {
        public static Response<object> ToEmptyData<T>(this Response<T> response)
            where T : class, new()
        {
            var emptyDataResponse = new Response<object>
            {
                Data = new object(),
                ErrorMessages = response.ErrorMessages
            };

            return emptyDataResponse;
        }
    }
}

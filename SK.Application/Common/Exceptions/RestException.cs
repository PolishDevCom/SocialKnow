using System;
using System.Net;

namespace SK.Application.Common.Exceptions
{
    public class RestException : Exception
    {
        public RestException(HttpStatusCode code, object errors)
        {
            Code = code;
            Errors = errors;
        }

        public HttpStatusCode Code { get; }
        public object Errors { get; }
    }
}

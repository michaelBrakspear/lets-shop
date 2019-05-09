using System;
using System.Collections.Generic;
using System.Net;

namespace LetsShop.Common
{
    public class UnexpectedStatusException : Exception
    {
        public UnexpectedStatusException(IEnumerable<HttpStatusCode> expectedStatusCodes, HttpStatusCode actualStatusCode, string responseContent) :
            base($"Expected status code(s) of {string.Join(',', expectedStatusCodes)} but received {actualStatusCode}", new Exception(responseContent))
        {
        }
    }
}
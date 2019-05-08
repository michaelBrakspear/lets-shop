using System;
using System.Net.Http;

namespace LetsShop.Client
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        private static void ConstructRequest()
        {

        }
    }
}

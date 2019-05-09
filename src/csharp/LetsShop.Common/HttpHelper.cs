using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LetsShop.Common
{
    public static class HttpHelper
    {
        public static HttpRequestMessage ConstructPost(Uri baseUri, object body)
        {
            var requestUri = new Uri(baseUri, "api/cart");
            var  request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Headers.Add("Accept", "application/json");
            request.SetContent(body);
            return request;
        }

        public static HttpRequestMessage ConstructRequestWithId(Uri baseUri, Guid id, HttpMethod method, object body = null)
        {
            var requestUri = new Uri(baseUri, $"api/cart/{id}");
            var request = new HttpRequestMessage(method, requestUri);
            request.Headers.Add("Accept", "application/json");
            if (body != null)
            {
                request.SetContent(body);
            }
            return request;
        }

        public static HttpRequestMessage ConstructRequestWithIdAndItemId(Uri baseUri, Guid id, Guid itemId, HttpMethod method)
        {
            var requestUri = new Uri(baseUri, $"api/cart/{id}/items/{itemId}");
            var request = new HttpRequestMessage(method, requestUri);
            request.Headers.Add("Accept", "application/json");
            return request;
        }

        public static HttpRequestMessage ConstructDeleteRequestForItems(Uri baseUri, Guid id)
        {
            var requestUri = new Uri(baseUri, $"api/cart/{id}/items");
            var request = new HttpRequestMessage(HttpMethod.Delete, requestUri);
            request.Headers.Add("Accept", "application/json");
            return request;
        }

        public static void SetContent(this HttpRequestMessage request, object obj)
        {
            request.Content = new StringContent(JsonConvert.SerializeObject(obj));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        public static async Task EnsureStatusCode(this HttpResponseMessage response,
            params HttpStatusCode[] expectedStatusCodes)
        {
            if (!expectedStatusCodes.Contains(response.StatusCode))
            {
                throw new UnexpectedStatusException(expectedStatusCodes, response.StatusCode,
                    await response.Content.ReadAsStringAsync());
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using LetsShop.Common.DTOs;
using Newtonsoft.Json;

namespace LetsShop.Basket.IntegrationTests
{
    internal static class sTestHelper
    {
        internal static HttpRequestMessage ConstructPost(Uri baseAddress)
        {
            var requestUri = new Uri(baseAddress, "api/cart");
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Headers.Add("Accept", "application/json");
            return request;
        }

        internal static HttpRequestMessage ConstructRequestWithId(Uri baseAddress, Guid id, HttpMethod method)
        {
            var requestUri = new Uri(baseAddress, $"api/cart/{id}");
            var request = new HttpRequestMessage(method, requestUri);
            request.Headers.Add("Accept", "application/json");
            return request;
        }

        internal static HttpRequestMessage ConstructRequestWithIdAndItemId(Uri baseAddress, Guid id, Guid itemId, HttpMethod method)
        {
            var requestUri = new Uri(baseAddress, $"api/cart/{id}/items/{itemId}");
            var request = new HttpRequestMessage(method, requestUri);
            request.Headers.Add("Accept", "application/json");
            return request;
        }

        internal static HttpRequestMessage ConstructDeleteRequestForItems(Uri baseAddress, Guid id)
        {
            var requestUri = new Uri(baseAddress, $"api/cart/{id}/items");
            var request = new HttpRequestMessage(HttpMethod.Delete, requestUri);
            request.Headers.Add("Accept", "application/json");
            return request;
        }

        internal static void SetContent(this HttpRequestMessage request, object obj)
        {
            request.Content = new StringContent(JsonConvert.SerializeObject(obj));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        internal static async Task CreateNewCartAsync(this HttpClient client, CartDto cart)
        {
            var req = ConstructPost(client.BaseAddress);
            req.SetContent(cart);
            await client.SendAsync(req);
        }

        internal static async Task<bool> CartExists(this HttpClient client, Guid id)
        {
            var req = ConstructRequestWithId(client.BaseAddress, id, HttpMethod.Get);
            var result = await client.SendAsync(req);
            return result.StatusCode != HttpStatusCode.NotFound;
        }

        internal static async Task<CartDto> GetCart(this HttpClient client, Guid id)
        {
            var req = ConstructRequestWithId(client.BaseAddress, id, HttpMethod.Get);
            var result = await client.SendAsync(req);
            return await result.Content.ReadAsAsync<CartDto>();
        }
    }
}

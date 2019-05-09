using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LetsShop.Common;
using LetsShop.Common.DTOs;

namespace LetsShop.Client
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _client;
        private readonly Uri _baseUri;

        public ApiClient(HttpClient client, string baseUri)
        {
            _client = client;
            _baseUri = new Uri(baseUri);
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        public async Task<CartDto> CreateCartAsync()
        {
            var requestMessage = HttpHelper.ConstructPost(_baseUri, CartDto.NewCart());

            var result = await _client.SendAsync(requestMessage);
            await result.EnsureStatusCode(HttpStatusCode.Created);

            return await result.Content.ReadAsAsync<CartDto>();
        }

        public async Task<CartDto> AddOrIncreaseProductAsync(Guid cartId, ProductDto product)
        {
            var requestMessage = HttpHelper.ConstructRequestWithId(_baseUri, cartId, HttpMethod.Put, product);

            var result = await _client.SendAsync(requestMessage);

            await result.EnsureStatusCode(HttpStatusCode.OK);

            return await result.Content.ReadAsAsync<CartDto>();
        }

        public async Task<CartDto> UpdateCartItemAsync(Guid cartId, ProductDto product)
        {
            var requestMessage = HttpHelper.ConstructRequestWithId(_baseUri, cartId, HttpMethod.Patch, product);

            var result = await _client.SendAsync(requestMessage);

            await result.EnsureStatusCode(HttpStatusCode.OK);

            return await result.Content.ReadAsAsync<CartDto>();
        }

        public async Task<CartDto> GetCartAsync(Guid cartId)
        {
            var requestMessage = HttpHelper.ConstructRequestWithId(_baseUri, cartId, HttpMethod.Get);

            var result = await _client.SendAsync(requestMessage);

            await result.EnsureStatusCode(HttpStatusCode.OK);

            return await result.Content.ReadAsAsync<CartDto>();
        }

        public async Task<CartDto> DeleteItem(Guid cartId, Guid productId)
        {
            var requestMessage = HttpHelper.ConstructRequestWithIdAndItemId(_baseUri, cartId, productId, HttpMethod.Delete);

            var result = await _client.SendAsync(requestMessage);

            await result.EnsureStatusCode(HttpStatusCode.OK);

            return await result.Content.ReadAsAsync<CartDto>();
        }

        public async Task DeleteCart(Guid cartId)
        {
            var requestMessage = HttpHelper.ConstructRequestWithId(_baseUri, cartId, HttpMethod.Get);

            var result = await _client.SendAsync(requestMessage);

            await result.EnsureStatusCode(HttpStatusCode.OK);
        }

        public async Task<CartDto> DeleteAllCartItems(Guid cartId)
        {
            var requestMessage = HttpHelper.ConstructDeleteRequestForItems(_baseUri, cartId);

            var result = await _client.SendAsync(requestMessage);

            await result.EnsureStatusCode(HttpStatusCode.OK);

            return await result.Content.ReadAsAsync<CartDto>();
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
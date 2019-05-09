using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LetsShop.Common.DTOs;

namespace LetsShop.Client
{
    public class Cart : IDisposable
    {
        private readonly IApiClient _client;
        private CartDto _cart;

        public Cart(string host) : this(new ApiClient(new HttpClient(new HttpClientHandler{ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true}), host))
        {
        }

        public Cart(IApiClient client)
        {
            _client = client;
        }

        public async Task InitialiseAsync(Guid? id = null)
        {
            _cart = id.HasValue ? await _client.GetCartAsync(id.Value) : await _client.CreateCartAsync();
        }

        public async Task AddProductAsync(Guid productId, int quantity, decimal price)
        {
            CheckCartInitialisation();

            _cart = await _client.AddOrIncreaseProductAsync(_cart.Id, new ProductDto { Id = productId, Quantity = quantity, Price = price});
        }

        public async Task UpdateProductAsync(Guid productId, int quantity)
        {
            CheckCartInitialisation();

            if (_cart.Products.Any(x => x.Id == productId))
            {
                _cart = await _client.UpdateCartItemAsync(_cart.Id, new ProductDto { Id = productId, Quantity = quantity });
            }
            else
            {
                throw new InvalidOperationException("Product does not exist in Cart");
            }
        }

        public async Task RemoveProductAsync(Guid productId)
        {
            CheckCartInitialisation();

            if (_cart.Products.Any(x => x.Id == productId))
            {
                _cart = await _client.DeleteItem(_cart.Id, productId);
            }
            else
            {
                throw new InvalidOperationException("Product does not exist in Cart");
            }
        }

        public async Task DeleteCart()
        {
            CheckCartInitialisation();

            await _client.DeleteCart(_cart.Id);
            _cart = null;
        }

        public async Task EmptyCart()
        {
            CheckCartInitialisation();

            _cart = await _client.DeleteAllCartItems(_cart.Id);
        }
        
        public IList<ProductDto> ListProducts()
        {
            CheckCartInitialisation();

            return _cart.Products;
        }

        private void CheckCartInitialisation()
        {
            if (_cart == null)
            {
                throw new CartInitialisationException();
            }
        }

        public decimal CurrentTotal => _cart?.RunningTotal ?? 0m;

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}

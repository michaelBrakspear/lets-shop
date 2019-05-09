using System;
using System.Threading.Tasks;
using LetsShop.Common.DTOs;

namespace LetsShop.Client
{
    public interface IApiClient : IDisposable
    {
        Task<CartDto> CreateCartAsync();
        Task<CartDto> AddOrIncreaseProductAsync(Guid cartId, ProductDto product);
        Task<CartDto> UpdateCartItemAsync(Guid cartId, ProductDto product);
        Task<CartDto> GetCartAsync(Guid cartId);
        Task<CartDto> DeleteItem(Guid cartId, Guid productId);
        Task<CartDto> DeleteAllCartItems(Guid cartId);
        Task DeleteCart(Guid cartId);
    }
}
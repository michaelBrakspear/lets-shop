using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetsShop.Basket.Domain.Entities;
using LetsShop.Basket.Domain.Exceptions;
using LetsShop.Basket.Domain.Repositories;

namespace LetsShop.Basket.Services
{
    public class CartService : ICartService
    {
        private readonly IRepository<Cart> _cartRepository;

        public CartService(IRepository<Cart> cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<Cart> GetCartAsync(Guid cartId)
        {
            return await RetrieveCartAsync(cartId);
        }

        public async Task<Cart> CreateCartAsync(Cart cart)
        {
            return await _cartRepository.AddOrUpdateAsync(cart);
        }

        public async Task<Cart> AddProductToCartAsync(Guid cartId, Product product)
        {
            var cart = await RetrieveCartAsync(cartId);
            var cartProduct = cart.Products.FirstOrDefault(x => x.Id == product.Id);

            if (cartProduct != null)
            {
                cartProduct.Quantity += product.Quantity;
            }
            else
            {
                cart.Products.Add(product);
            }
           
            return await _cartRepository.AddOrUpdateAsync(cart);
        }

        public async Task<Cart> UpdateProductInCartAsync(Guid cartId, Product product)
        {
            var cart = await RetrieveCartAsync(cartId);

            var cartProduct = RetrieveProduct(cart, product.Id);

            if (product.Quantity == 0)
            {
                cart.Products.Remove(cartProduct);
            }
            else
            {
                cartProduct.Quantity = product.Quantity;
            }

            return await _cartRepository.AddOrUpdateAsync(cart);
        }

        public async Task<Cart> DeleteProductAsync(Guid cartId, Guid productId)
        {
            var cart = await RetrieveCartAsync(cartId);
            var cartProduct = RetrieveProduct(cart, productId);
            cart.Products.Remove(cartProduct);
            return await _cartRepository.AddOrUpdateAsync(cart);
        }

        public async Task<Cart> DeleteProductsAsync(Guid cartId)
        {
            var cart = await RetrieveCartAsync(cartId);
            cart.Products = new List<Product>();
            return await _cartRepository.AddOrUpdateAsync(cart);
        }

        public async Task DeleteCartAsync(Guid cartId)
        {
            var cart = await RetrieveCartAsync(cartId);
            await _cartRepository.DeleteById(cart.Id);
        }

        private async Task<Cart> RetrieveCartAsync(Guid cartId)
        {
            var cart = await _cartRepository.GetByIdAsync(cartId);
            if (cart == null)
            {
                throw new CartNotFoundException(cartId);
            }
            return cart;
        }

        private static Product RetrieveProduct(Cart cart, Guid productId)
        {
            var product = cart.Products.FirstOrDefault(x => x.Id == productId);
            if (product == null)
            {
                throw new ProductNotFoundException(productId);
            }
            return product;
        }
    }

    public interface ICartService
    {
        Task<Cart> GetCartAsync(Guid cartId);
        Task<Cart> CreateCartAsync(Cart cart);
        Task<Cart> AddProductToCartAsync(Guid cartId, Product product);
        Task<Cart> UpdateProductInCartAsync(Guid cartId, Product product);
        Task<Cart> DeleteProductAsync(Guid cartId, Guid productId);
        Task<Cart> DeleteProductsAsync(Guid cartId);
        Task DeleteCartAsync(Guid cartId);

    }
}

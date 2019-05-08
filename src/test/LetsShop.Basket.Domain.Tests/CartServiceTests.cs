using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetsShop.Basket.Domain.Entities;
using LetsShop.Basket.Domain.Exceptions;
using LetsShop.Basket.Domain.Repositories;
using LetsShop.Basket.Services;
using Moq;
using Xunit;

namespace LetsShop.Basket.Domain.Tests
{
    public class CartServiceTests
    {
        private Mock<IRepository<Cart>> _mockCartRepo;
        private CartService _service;

        public CartServiceTests()
        {
            _mockCartRepo = new Mock<IRepository<Cart>>();
            _service = new CartService(_mockCartRepo.Object);
        }

        [Fact]
        public async Task GetCartAsync_ThrowsExceptionIfCartDoesNotExist()
        {
            await Assert.ThrowsAsync<CartNotFoundException>(() => _service.GetCartAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task GetCartAsync_ReturnsCart()
        {
            // arrange
            var cartId = Guid.NewGuid();
            var cart = new Cart
            {
                Id = cartId,
            };
            _mockCartRepo.Setup(x => x.GetByIdAsync(cartId)).ReturnsAsync(cart);

            // act
            var result = await _service.GetCartAsync(cartId);

            // assert
            Assert.Equal(cart, result);
            _mockCartRepo.Verify(x => x.GetByIdAsync(cartId), Times.Once);
        }

        [Fact]
        public async Task CreateCartAsync_CallsAddOrUpdateAsync()
        {
            // arrange
            var cart = new Cart {Id = Guid.NewGuid()};

            // act
            await _service.CreateCartAsync(cart);

            // assert
            _mockCartRepo.Verify(x=> x.AddOrUpdateAsync(cart), Times.Once);
        }

        [Fact]
        public async Task AddProductToCartAsync_ThrowsExceptionIfCartDoesNotExist()
        {
            await Assert.ThrowsAsync<CartNotFoundException>(() => _service.AddProductToCartAsync(Guid.NewGuid(), null));
        }

        [Fact]
        public async Task AddProductToCartAsync_UpdatesProductQtyIfProductIsAlreadyInCart()
        {
            // arrange
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var cart = new Cart
            {
                Id = cartId,
                Products = new List<Product> { new Product { Id = productId, Quantity = 10 }}
            };
            _mockCartRepo.Setup(x => x.GetByIdAsync(cartId)).ReturnsAsync(cart);

            // act
            await _service.AddProductToCartAsync(cartId, new Product{Id = productId, Quantity = 1});

            // assert
            _mockCartRepo.Verify(x=> x.GetByIdAsync(cartId), Times.Once);
            _mockCartRepo.Verify(
                x => x.AddOrUpdateAsync(It.Is<Cart>(c =>
                    c.Id == cartId && c.Products.Any(p => p.Id == productId && p.Quantity == 11))), Times.Once);
        }

        [Fact]
        public async Task AddProductToCartAsync_AddProductToCart()
        {
            // arrange
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Id = productId,
                Quantity = 1
            };
            var cart = new Cart
            {
                Id = cartId,
                Products = new List<Product>()
            };
            _mockCartRepo.Setup(x => x.GetByIdAsync(cartId)).ReturnsAsync(cart);

            // act
            await _service.AddProductToCartAsync(cartId, product);

            // assert
            _mockCartRepo.Verify(x => x.GetByIdAsync(cartId), Times.Once);
            _mockCartRepo.Verify(
                x => x.AddOrUpdateAsync(It.Is<Cart>(c =>
                    c.Id == cartId && c.Products.Any(p => p.Id == productId && p.Quantity == product.Quantity))), Times.Once);
        }

        [Fact]
        public async Task UpdateProductInCartAsync_RemovesProductIfQuantityIsZero()
        {
            // arrange
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Id = productId,
                Quantity = 0
            };
            var cart = new Cart
            {
                Id = cartId,
                Products = new List<Product>{ new Product{ Id = productId, Quantity = 10 } }
            };  
            _mockCartRepo.Setup(x => x.GetByIdAsync(cartId)).ReturnsAsync(cart);

            // act
            await _service.UpdateProductInCartAsync(cartId, product);

            // assert
            _mockCartRepo.Verify(x => x.GetByIdAsync(cartId), Times.Once);
            _mockCartRepo.Verify(x => x.AddOrUpdateAsync(It.Is<Cart>(c => c.Id == cartId && c.Products.Count == 0)), Times.Once);
        }

        [Fact]
        public async Task UpdateProductInCartAsync_ReplacesItemQuantityIfItemExists()
        {
            // arrange
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Id = productId,
                Quantity = 5
            };
            var cart = new Cart
            {
                Id = cartId,
                Products = new List<Product> { new Product { Id = productId, Quantity = 10 } }
            };
            _mockCartRepo.Setup(x => x.GetByIdAsync(cartId)).ReturnsAsync(cart);

            // act
            await _service.UpdateProductInCartAsync(cartId, product);

            // assert
            _mockCartRepo.Verify(x => x.GetByIdAsync(cartId), Times.Once);
            _mockCartRepo.Verify(
                x => x.AddOrUpdateAsync(It.Is<Cart>(c =>
                    c.Id == cartId && c.Products.First(p => p.Id == productId).Quantity == 5)), Times.Once);
        }

        [Fact]
        public async Task UpdateProductInCartAsync_ThrowsExceptionIfProductDoesNotExistInCart()
        {
            // arrange
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Id = productId,
                Quantity = 5
            };
            var cart = new Cart
            {
                Id = cartId,
                Products = new List<Product>()
            };
            _mockCartRepo.Setup(x => x.GetByIdAsync(cartId)).ReturnsAsync(cart);

            // act / assert
            await Assert.ThrowsAsync<ProductNotFoundException>(() => _service.UpdateProductInCartAsync(cartId, product));
        }

        [Fact]
        public async Task DeleteProductAsync_RemovesProductFromCart()
        {
            // arrange
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var productToDelete = new Product
            {
                Id = productId,
                Quantity = 5
            };
            var productToKeep = new Product
            {
                Id = Guid.NewGuid(),
                Quantity = 1
            };
            var cart = new Cart
            {
                Id = cartId,
                Products = new List<Product> { productToDelete, productToKeep }
            };
            _mockCartRepo.Setup(x => x.GetByIdAsync(cartId)).ReturnsAsync(cart);

            // act
            await _service.DeleteProductAsync(cartId, productId);

            // assert
            _mockCartRepo.Verify(x => x.AddOrUpdateAsync(It.Is<Cart>(c =>
                c.Id == cartId && c.Products.Any(p => p.Id == productToKeep.Id) && c.Products.All(p => p.Id != productId))), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_ThrowsExceptionIfProductDoesNotExistInCart()
        {
            // arrange
            var cartId = Guid.NewGuid();
            var cart = new Cart
            {
                Id = cartId,
                Products = new List<Product>()
            };
            _mockCartRepo.Setup(x => x.GetByIdAsync(cartId)).ReturnsAsync(cart);

            // act / assert
            await Assert.ThrowsAsync<ProductNotFoundException>(() => _service.DeleteProductAsync(cartId, Guid.NewGuid()));
        }

        [Fact]
        public async Task DeleteProductsAsync_RemovesAllProductsFromCart()
        {
            // arrange
            var cartId = Guid.NewGuid();
            var product1 = new Product
            {
                Id = Guid.NewGuid(),
                Quantity = 5
            };
            var product2 = new Product
            {
                Id = Guid.NewGuid(),
                Quantity = 1
            };
            var cart = new Cart
            {
                Id = cartId,
                Products = new List<Product> { product1, product2 }
            };
            _mockCartRepo.Setup(x => x.GetByIdAsync(cartId)).ReturnsAsync(cart);

            // act
            await _service.DeleteProductsAsync(cartId);

            // assert
            _mockCartRepo.Verify(x => x.AddOrUpdateAsync(It.Is<Cart>(c =>
                c.Id == cartId && !c.Products.Any())), Times.Once);
        }

        [Fact]
        public async Task DeleteCartAsync_DeletesCart()
        {
            // arrange
            var cartId = Guid.NewGuid();
            var cart = new Cart
            {
                Id = cartId
            };
            _mockCartRepo.Setup(x => x.GetByIdAsync(cartId)).ReturnsAsync(cart);

            // act
            await _service.DeleteCartAsync(cartId);

            // assert
            _mockCartRepo.Verify(x => x.DeleteById(cartId), Times.Once);
        }

    }
}

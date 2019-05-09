using System;
using System.Threading.Tasks;
using LetsShop.Common.DTOs;
using Moq;
using Xunit;

namespace LetsShop.Client.Tests
{
    public class CartTests
    {
        private readonly Mock<IApiClient> _mockClient;
        private readonly Cart _cart;

        public CartTests()
        {
            _mockClient = new Mock<IApiClient>();
            _cart = new Cart(_mockClient.Object);
        }

        [Fact]
        public async Task InitialiseAsync_CreatesCartIfIdIsNotProvided()
        {
            // arrange
            _mockClient.Setup(x => x.CreateCartAsync()).ReturnsAsync(new CartDto());

            // act 
            await _cart.InitialiseAsync();

            // assert 
            _mockClient.Verify(x=> x.CreateCartAsync(), Times.Once);
        }

        [Fact]
        public async Task InitialiseAsync_GetsCartIfIdIsProvided()
        {
            // arrange
            var cartId = Guid.NewGuid();
            _mockClient.Setup(x => x.GetCartAsync(cartId)).ReturnsAsync(new CartDto());

            // act 
            await _cart.InitialiseAsync(cartId);

            // assert 
            _mockClient.Verify(x => x.GetCartAsync(cartId), Times.Once);
        }

        [Fact]
        public async Task CartInitialisationException_IsThrown()
        {
            // act / assert 
            await Assert.ThrowsAsync<CartInitialisationException>(() => _cart.AddProductAsync(Guid.NewGuid(), 1, 1m));
            await Assert.ThrowsAsync<CartInitialisationException>(() => _cart.RemoveProductAsync(Guid.NewGuid()));
            await Assert.ThrowsAsync<CartInitialisationException>(() => _cart.UpdateProductAsync(Guid.NewGuid(), 1));
            await Assert.ThrowsAsync<CartInitialisationException>(() => _cart.DeleteCart());
            await Assert.ThrowsAsync<CartInitialisationException>(() => _cart.EmptyCart());
        }

        [Fact]
        public async Task AddProductAsync_CallsAddOrUpdateOnTheClient()
        {
            // arrange 
            var cart = await InitCart();
            var productId = Guid.NewGuid();
            var price = 15m;
            var quantity = 10;

            // act
            await _cart.AddProductAsync(productId, quantity, price);

            // assert
            _mockClient.Verify(x => x.AddOrIncreaseProductAsync(cart.Id, MatchProduct(productId, quantity, price)));
        }

        [Fact]
        public async Task UpdateProductAsync_CallsUpdateOnTheClient()
        {
            // arrange 
            var productId = Guid.NewGuid();
            var quantity = 10;
            var cart = await InitCart(new ProductDto{Id = productId, Quantity = quantity});
            
            // act
            await _cart.UpdateProductAsync(productId, quantity);

            // assert
            _mockClient.Verify(x => x.UpdateCartItemAsync(cart.Id, It.Is<ProductDto>(p => p.Id == productId && p.Quantity == quantity)));
        }

        [Fact]
        public async Task UpdateProductAsync_ThrowsExceptionIfItemDoesntExistInCart()
        {
            // arrange 
            var cart = await InitCart();

            // act / assert
            await Assert.ThrowsAsync<InvalidOperationException>(()=> _cart.UpdateProductAsync(Guid.NewGuid(), 1));
        }

        [Fact]
        public async Task RemoveProductAsync_CallsDeleteOnTheClient()
        {
            // arrange 
            var productId = Guid.NewGuid();
            var quantity = 10;
            var cart = await InitCart(new ProductDto { Id = productId, Quantity = quantity });

            // act
            await _cart.RemoveProductAsync(productId);

            // assert
            _mockClient.Verify(x => x.DeleteItem(cart.Id, productId));
        }

        [Fact]
        public async Task RemoveProductAsync_ThrowsExceptionIfItemDoesntExistInCart()
        {
            // arrange 
            await InitCart();

            // act / assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _cart.UpdateProductAsync(Guid.NewGuid(), 1));
        }

        [Fact]
        public async Task DeleteCart_CallsDeleteCartOnTheClient()
        {
            // arrange
            var cart = await InitCart();

            // act
            await _cart.DeleteCart();

            // assert 
            _mockClient.Verify(x=> x.DeleteCart(cart.Id), Times.Once);
        }

        [Fact]
        public async Task EmptyCart_CallsDeleteAllCartItemsOnTheClient()
        {
            // arrange
            var cart = await InitCart();

            // act
            await _cart.EmptyCart();

            // assert 
            _mockClient.Verify(x => x.DeleteAllCartItems(cart.Id), Times.Once);
        }

        [Fact]
        public async Task ListProducts_ReturnsCartProducts()
        {
            // 
            var product = ProductDto.NewProduct();
            var cart = await InitCart(product);

            // act
            var list = _cart.ListProducts();

            // assert 
            Assert.Contains(product, list);
        }

        private static ProductDto MatchProduct(Guid productId, int quantity, decimal price)
        {
            return It.Is<ProductDto>(p => p.Id == productId && p.Quantity == quantity && p.Price == price);
        }

        private async Task<CartDto> InitCart(ProductDto product = null)
        {
            var cart = CartDto.NewCart();
            if(product != null) cart.Products.Add(product);
            _mockClient.Setup(x => x.CreateCartAsync()).ReturnsAsync(cart);
            await _cart.InitialiseAsync();
            return cart;
        }

    }
}

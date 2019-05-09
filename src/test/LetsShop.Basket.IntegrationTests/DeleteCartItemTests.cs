using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LetsShop.Basket.Domain.IntegrationTests;
using LetsShop.Common;
using LetsShop.Common.DTOs;
using Xunit;

namespace LetsShop.Basket.IntegrationTests
{
    public class DeleteCartItemTests : IClassFixture<WebHostTestFixture<TestStartUp>>
    {
        private readonly WebHostTestFixture<TestStartUp> _fixture;

        public DeleteCartItemTests(WebHostTestFixture<TestStartUp> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task DELETE_CartItem_ReturnsBadRequestIfProductDoesNotExistInCart()
        {
            // arrange
            var cart = CartDto.NewCart();
            await _fixture.Client.CreateNewCartAsync(cart);
            var request = HttpHelper.ConstructRequestWithIdAndItemId(_fixture.Client.BaseAddress, cart.Id, 
                Guid.NewGuid(), HttpMethod.Delete);

            // act
            var result = await _fixture.Client.SendAsync(request);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task DELETE_CartItem_ReturnsNotFoundIfCartDoesNotExist()
        {
            // arrange
            var request = HttpHelper.ConstructRequestWithIdAndItemId(_fixture.Client.BaseAddress, Guid.NewGuid(),
                Guid.NewGuid(), HttpMethod.Delete);

            // act
            var result = await _fixture.Client.SendAsync(request);

            // assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task DELETE_CartItem_DeletesCartItem()
        {
            // arrange
            var cart = CartDto.NewCart();
            var product =ProductDto.NewProduct();
            cart.Products.Add(product);
            await _fixture.Client.CreateNewCartAsync(cart);

            var request = HttpHelper.ConstructRequestWithIdAndItemId(_fixture.Client.BaseAddress, cart.Id, product.Id, HttpMethod.Delete);

            // act
            var response = await _fixture.Client.SendAsync(request);

            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadAsAsync<CartDto>();
            Assert.DoesNotContain(result.Products, x=> x.Id == product.Id);
        }
    }
}
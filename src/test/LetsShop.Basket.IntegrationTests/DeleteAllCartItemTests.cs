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
    public class DeleteAllCartItemTests : IClassFixture<WebHostTestFixture<TestStartUp>>
    {
        private readonly WebHostTestFixture<TestStartUp> _fixture;

        public DeleteAllCartItemTests(WebHostTestFixture<TestStartUp> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task DELETE_CartItems_ReturnsNotFoundIfCartDoesNotExist()
        {
            // arrange
            var request = HttpHelper.ConstructDeleteRequestForItems(_fixture.Client.BaseAddress, Guid.NewGuid());

            // act
            var result = await _fixture.Client.SendAsync(request);

            // assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task DELETE_CartItems_DeletesAllItems()
        {
            // arrange
            var cart = CartDto.NewCart();
            var product = ProductDto.NewProduct();
            var product2 = ProductDto.NewProduct();
            var product3 = ProductDto.NewProduct();
            cart.Products.Add(product);
            cart.Products.Add(product2);
            cart.Products.Add(product3);
            await _fixture.Client.CreateNewCartAsync(cart);

            var request = HttpHelper.ConstructDeleteRequestForItems(_fixture.Client.BaseAddress, cart.Id);

            // act
            var response = await _fixture.Client.SendAsync(request);

            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadAsAsync<CartDto>();
            Assert.Empty(result.Products);
        }
    }
}
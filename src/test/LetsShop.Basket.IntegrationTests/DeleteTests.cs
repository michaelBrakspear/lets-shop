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
    public class DeleteTests : IClassFixture<WebHostTestFixture<TestStartUp>>
    {
        private readonly WebHostTestFixture<TestStartUp> _fixture;

        public DeleteTests(WebHostTestFixture<TestStartUp> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task DELETE_ReturnsNotFoundIfCartDoesNotExist()
        {
            // arrange
            var request = HttpHelper.ConstructRequestWithId(_fixture.Client.BaseAddress, Guid.NewGuid(), HttpMethod.Delete);

            // act
            var result = await _fixture.Client.SendAsync(request);

            // assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task DELETE_DeletesCart()
        {
            // arrange
            var cart = CartDto.NewCart();
            await _fixture.Client.CreateNewCartAsync(cart);

            var request = HttpHelper.ConstructRequestWithId(_fixture.Client.BaseAddress, cart.Id, HttpMethod.Delete);

            // act
            var response = await _fixture.Client.SendAsync(request);

            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.False(await _fixture.Client.CartExists(cart.Id));
        }
    }
}
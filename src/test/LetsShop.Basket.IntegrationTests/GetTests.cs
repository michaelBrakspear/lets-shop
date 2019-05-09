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
    public class GetTests : IClassFixture<WebHostTestFixture<TestStartUp>>
    {
        private readonly WebHostTestFixture<TestStartUp> _fixture;

        public GetTests(WebHostTestFixture<TestStartUp> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GET_ReturnsNotFoundIfCartNotBeenCreated()
        {
            // arrange
            var request = HttpHelper.ConstructRequestWithId(_fixture.Client.BaseAddress, Guid.NewGuid(), HttpMethod.Get);

            // act
            var result = await _fixture.Client.SendAsync(request);

            // assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task GET_ReturnsCartIfExists()
        {
            // arrange
            var cart = CartDto.NewCart();
            await _fixture.Client.CreateNewCartAsync(cart);
            var request = HttpHelper.ConstructRequestWithId(_fixture.Client.BaseAddress, cart.Id, HttpMethod.Get);

            // act
            var response = await _fixture.Client.SendAsync(request);

            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadAsAsync<CartDto>();
            Assert.Equal(cart.Id, result.Id);
        }
    }
}
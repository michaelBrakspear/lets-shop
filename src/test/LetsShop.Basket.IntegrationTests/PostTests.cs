using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LetsShop.Basket.Domain.IntegrationTests;
using LetsShop.Common.DTOs;
using Xunit;

namespace LetsShop.Basket.IntegrationTests
{
    public class PostTests : IClassFixture<WebHostTestFixture<TestStartUp>>
    {
        private readonly WebHostTestFixture<TestStartUp> _fixture;

        public PostTests(WebHostTestFixture<TestStartUp> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task POST_WithEmptyBodyCreatesCart()
        {
            // arrange
            var request = TestHelper.ConstructPost(_fixture.Client.BaseAddress);
            request.SetContent(new object());

            // act
            var result = await _fixture.Client.SendAsync(request);

            // assert
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            var cart = await result.Content.ReadAsAsync<CartDto>();
            Assert.NotNull(cart);
            Assert.NotEqual(Guid.Empty, cart.Id);
        }

        [Fact]
        public async Task POST_CreatesCart()
        {
            // arrange
            var cart = CartDto.NewCart();
            var request = TestHelper.ConstructPost(_fixture.Client.BaseAddress);
            request.SetContent(cart);

            // act
            var response = await _fixture.Client.SendAsync(request);

            // assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var result = await response.Content.ReadAsAsync<CartDto>();
            Assert.Equal(cart.Id, result.Id);
        }
    }
}

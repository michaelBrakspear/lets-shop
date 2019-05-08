using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LetsShop.Basket.Domain.IntegrationTests;
using LetsShop.Common.DTOs;
using Xunit;

namespace LetsShop.Basket.IntegrationTests
{
    public class PutTests : IClassFixture<WebHostTestFixture<TestStartUp>>
    {
        private readonly WebHostTestFixture<TestStartUp> _fixture;

        public PutTests(WebHostTestFixture<TestStartUp> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task PUT_ReturnsBadRequestIfNullProduct()
        {
            // arrange
            var request = TestHelper.ConstructRequestWithId(_fixture.Client.BaseAddress, Guid.NewGuid(), HttpMethod.Put);
            request.SetContent(new object());

            // act
            var result = await _fixture.Client.SendAsync(request);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task PUT_ReturnsBadRequestIfQuantityIsZero()
        {
            // arrange
            var request = TestHelper.ConstructRequestWithId(_fixture.Client.BaseAddress, Guid.NewGuid(), HttpMethod.Put);
            request.SetContent(new ProductDto{Id = Guid.NewGuid(), Quantity = 0});

            // act
            var result = await _fixture.Client.SendAsync(request);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }


        [Fact]
        public async Task PUT_ReturnsNotFoundIfCartNotBeenCreated()
        {
            // arrange
            var request = TestHelper.ConstructRequestWithId(_fixture.Client.BaseAddress, Guid.NewGuid(), HttpMethod.Put);
            request.SetContent(new ProductDto { Id = Guid.NewGuid(), Quantity = 10 });

            // act
            var response = await _fixture.Client.SendAsync(request);

            // assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PUT_AddsProductToBasket()
        {
            // arrange
            var cart = CartDto.NewCart();
            var product = ProductDto.NewProduct();
            await _fixture.Client.CreateNewCartAsync(cart);

            var request = TestHelper.ConstructRequestWithId(_fixture.Client.BaseAddress, cart.Id, HttpMethod.Put);
            request.SetContent(product);

            // act
            var response = await _fixture.Client.SendAsync(request);

            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadAsAsync<CartDto>();
            Assert.Equal(1, result.Products.Count);
            Assert.Contains(result.Products, x => x.Id == product.Id && x.Quantity == product.Quantity && x.Price == product.Price);
        }
    }
}
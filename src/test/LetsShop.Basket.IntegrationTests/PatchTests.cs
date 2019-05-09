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
    public class PatchTests : IClassFixture<WebHostTestFixture<TestStartUp>>
    {
        private readonly WebHostTestFixture<TestStartUp> _fixture;

        public PatchTests(WebHostTestFixture<TestStartUp> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task PATCH_NotFoundIfCartDoesNotExist()
        {
            // arrange
            var request = HttpHelper.ConstructRequestWithId(_fixture.Client.BaseAddress, Guid.NewGuid(), HttpMethod.Patch);
            request.SetContent(ProductDto.NewProduct());

            // act
            var result = await _fixture.Client.SendAsync(request);

            // assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task PATCH_ReturnsBadRequestIfNoCartProductToUpdate()
        {
            // arrange
            var cart = CartDto.NewCart();
            await _fixture.Client.CreateNewCartAsync(cart);

            var request = HttpHelper.ConstructRequestWithId(_fixture.Client.BaseAddress, cart.Id, HttpMethod.Patch);
            request.SetContent(new ProductDto { Id = Guid.NewGuid(), Quantity = 10 });

            // act
            var response = await _fixture.Client.SendAsync(request);

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PATCH_UpdatesProductQuantity()
        {
            // arrange
            var cart = CartDto.NewCart();
            var product = ProductDto.NewProduct(5);
            cart.Products.Add(product);
            product.Quantity = 10;
            await _fixture.Client.CreateNewCartAsync(cart);

            var request = HttpHelper.ConstructRequestWithId(_fixture.Client.BaseAddress, cart.Id, HttpMethod.Patch);
            request.SetContent(product);

            // act
            var response = await _fixture.Client.SendAsync(request);

            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadAsAsync<CartDto>();
            Assert.Equal(1, result.Products.Count);
            Assert.Contains(result.Products, x => x.Id == product.Id && x.Quantity == product.Quantity);
        }
    }
}
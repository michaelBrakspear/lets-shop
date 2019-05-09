using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LetsShop.Basket.Domain.Entities;
using LetsShop.Basket.Services;
using LetsShop.Common.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LetsShop.Basket.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public CartController(ICartService cartService, IMapper mapper)
        {
            _cartService = cartService;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = nameof(GetCart))]
        [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCart(Guid id)
        {
            var cart = await _cartService.GetCartAsync(id);
            var cartDto = CreateLinksForUser(_mapper.Map<CartDto>(cart));
            return Ok(cartDto);
        }

        [HttpPost(Name = nameof(CreateCart))]
        [ProducesResponseType(typeof(CartDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCart([FromBody] CartDto cart)
        {
            var result = await _cartService.CreateCartAsync(_mapper.Map<Cart>(cart ?? CartDto.NewCart()));
            var cartDto = CreateLinksForUser(_mapper.Map<CartDto>(result));
            return Created(Url.Action("GetCart", new { id = cartDto.Id }), cartDto);
        }

        [HttpPut("{id}", Name = nameof(AddToCart))]
        [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddToCart(Guid id, [FromBody] ProductDto productDto)
        {
            if (productDto?.Quantity == 0)
            {
                return BadRequest();
            }

            var updatedCart = await _cartService.AddProductToCartAsync(id, _mapper.Map<Product>(productDto));
            return Ok(_mapper.Map<CartDto>(updatedCart));
        }

        [HttpPatch("{id}", Name = nameof(UpdateCartItem))]
        [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCartItem(Guid id, [FromBody] ProductDto product)
        {
            var updatedCart = await _cartService.UpdateProductInCartAsync(id, _mapper.Map<Product>(product));
            return Ok(_mapper.Map<CartDto>(updatedCart));
        }

        [HttpDelete("{id}", Name = nameof(DeleteCart))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCart(Guid id)
        {
            await _cartService.DeleteCartAsync(id);
            return Ok();
        }

        [HttpDelete("{id}/items/{productId}", Name = nameof(DeleteCartItem))]
        [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCartItem(Guid id, Guid productId)
        {
            var cart = await _cartService.DeleteProductAsync(id, productId);
            return Ok(_mapper.Map<CartDto>(cart));
        }

        [HttpDelete("{id}/items", Name = nameof(DeleteCartItems))]
        [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCartItems(Guid id)
        {
            var cart = await _cartService.DeleteProductsAsync(id);
            return Ok(_mapper.Map<CartDto>(cart));
        }

        private CartDto CreateLinksForUser(CartDto cart)
        {
            var idObj = new { id = cart.Id };

            cart.Links = new List<LinkDto>
            {
                new LinkDto(Url.Action("GetCart", idObj), "self", "GET"),
                new LinkDto(Url.Action("AddToCart", idObj), "update_cart", "PUT"),
                new LinkDto(Url.Action("DeleteCart", idObj), "delete_cart", "DELETE")
            };

            return cart;
        }
    }
}
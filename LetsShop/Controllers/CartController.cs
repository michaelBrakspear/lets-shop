using System;
using System.Collections.Generic;
using LetsShop.Basket.WebApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LetsShop.Basket.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        [HttpGet]
        public Cart Get()
        {
            return new Cart { Id = Guid.NewGuid() };
        }

        [HttpGet("{id}", Name = nameof(GetCart))]
        public Cart GetCart(Guid id)
        {
            return new Cart { Id = id };
        }

        [HttpPost]
        public void Post([FromBody]Cart value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Product product)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
        }
    }
}

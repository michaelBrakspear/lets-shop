using System;
using System.Collections.Generic;

namespace LetsShop.Common.DTOs
{
    public class CartDto
    {
        public static CartDto NewCart()
        {
            return new CartDto
            {
                Id = Guid.NewGuid(),
                Products = new List<ProductDto>(),
                Links = new List<LinkDto>()
            };
        }

        public Guid Id { get; set; }
        public IList<ProductDto> Products { get; set; }
        public decimal RunningTotal { get; set; }
        public IList<LinkDto> Links { get; set; }
    }
}
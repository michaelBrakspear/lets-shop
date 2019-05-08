using System;

namespace LetsShop.Common.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
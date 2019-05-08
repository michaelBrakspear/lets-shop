using System;

namespace LetsShop.Common.DTOs
{
    public class ProductDto
    {
        public static ProductDto NewProduct(int qty = 1, decimal price = 10m)
        {
            return new ProductDto
            {
                Id = Guid.NewGuid(),
                Price = price,
                Quantity = qty
            };
        }


        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
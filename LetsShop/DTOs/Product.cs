using System;
using System.Collections.Generic;

namespace LetsShop.Basket.WebApi.DTOs
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class Customer
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
    }

    public class Cart
    {
        public Guid Id { get; set; }
        public Customer Customer { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public decimal RunningTotal { get; set; }
    }
}

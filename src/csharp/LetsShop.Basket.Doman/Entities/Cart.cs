using System;
using System.Collections.Generic;

namespace LetsShop.Basket.Domain.Entities
{
    public class Cart : IEntity
    {
        public Guid Id { get; set; }
        public IList<Product> Products { get; set; }
    }

    public class Product : IEntity
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public interface IEntity
    {
        Guid Id { get; set; }
    }
}

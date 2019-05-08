using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using LetsShop.Basket.Domain.Entities;

namespace LetsShop.Basket.WebApi.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

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

    public class LinkDto
    {
        public string Href { get; private set; }
        public string Rel { get; private set; }
        public string Method { get; private set; }
        public LinkDto(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }
}

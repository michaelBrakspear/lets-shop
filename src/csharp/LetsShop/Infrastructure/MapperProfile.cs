using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LetsShop.Basket.Domain.Entities;
using LetsShop.Basket.WebApi.DTOs;

namespace LetsShop.Basket.WebApi.Infrastructure
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Cart, CartDto>().ForMember(x => x.RunningTotal,
                opt => opt.MapFrom(x => x.Products.Sum(p => p.Price*p.Quantity)));

            CreateMap<CartDto, Cart>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id == Guid.Empty ? Guid.NewGuid() : x.Id))
                .ForMember(x => x.Products, opt => opt.MapFrom(x => x.Products ?? new List<ProductDto>()));

            CreateMap<Product, ProductDto>();
        }
    }
}

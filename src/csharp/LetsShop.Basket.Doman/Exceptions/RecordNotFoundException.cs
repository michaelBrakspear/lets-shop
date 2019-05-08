using System;
using LetsShop.Basket.Domain.Entities;

namespace LetsShop.Basket.Domain.Exceptions
{
    public abstract class RecordNotFoundException : Exception
    {
        protected RecordNotFoundException(Type type, Guid id) : base($"No record for type {type} with id {id} found")
        { } 
    }

    public class CartNotFoundException : RecordNotFoundException
    {
        public CartNotFoundException(Guid id) : base(typeof(Cart), id)
        {
        }
    }

    public class ProductNotFoundException : RecordNotFoundException
    {
        public ProductNotFoundException(Guid id) : base(typeof(Product), id)
        {
        }
    }
}
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using LetsShop.Basket.Domain.Entities;
using LetsShop.Basket.Domain.Exceptions;

namespace LetsShop.Basket.Domain.Repositories
{
    public class InMemoryCartRepository : IRepository<Cart>
    {
        private readonly ConcurrentDictionary<Guid, Cart> _cache = new ConcurrentDictionary<Guid, Cart>();

        public async Task<Cart> GetByIdAsync(Guid id)
        {
            // no logic here. let's assume our data source is always available
            _cache.TryGetValue(id, out var cart);
            return await Task.FromResult(cart);
        }

        public async Task<Cart> AddOrUpdateAsync(Cart entity)
        {
            _cache[entity.Id] = entity;

            return await Task.FromResult(_cache[entity.Id]);
        }

        public Task DeleteById(Guid id)
        {
            _cache.TryRemove(id, out var ignore);
            return Task.CompletedTask;
        }
    }

    public interface IRepository<T> where T : IEntity
    {
        Task<T> GetByIdAsync(Guid id);

        Task<T> AddOrUpdateAsync(T entity);
        Task DeleteById(Guid id);
    }
}

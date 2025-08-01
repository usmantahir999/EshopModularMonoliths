﻿
using Basket.Data.JsonConverters;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Basket.Data.Repository
{
    public class CachedBasketRepository(IBasketRepository basketRepository, IDistributedCache cache) : IBasketRepository
    {
        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition= JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new ShoppingCartConverter(),
                new ShoppingCartItemConverter()
            }
        };
        public async Task<ShoppingCart> GetBasket(string userName, bool asNoTracking = true, CancellationToken cancellationToken = default)
        {
            if (!asNoTracking)
            {
                return await basketRepository.GetBasket(userName, asNoTracking, cancellationToken);
            }

            var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
            if(!string.IsNullOrEmpty(cachedBasket))
            {
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket, _options)!;
            }
            var basket = await basketRepository.GetBasket(userName, asNoTracking, cancellationToken);
            await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket, _options), cancellationToken);
            return basket;

        }
        public async Task<ShoppingCart> CreateBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            await basketRepository.CreateBasket(basket, cancellationToken);
            await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket, _options), cancellationToken);
            return basket;
        }

        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            await basketRepository.DeleteBasket(userName, cancellationToken);
            await cache.RemoveAsync(userName, cancellationToken);
            return true;
        }

        public async Task<int> SaveChangesAsync(string? userName=null,CancellationToken cancellationToken = default)
        {
            var result = await basketRepository.SaveChangesAsync(userName, cancellationToken);
            if (userName is not null)
            {
                await cache.RemoveAsync(userName, cancellationToken);
            }

            return result;
        }
    }
}

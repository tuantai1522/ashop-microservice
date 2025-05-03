using Basket.API.Entities;
using Microsoft.Extensions.Caching.Hybrid;
using Newtonsoft.Json;

namespace Basket.API.Data;

public class CachedBasketRepository(IBasketRepository basketRepository, HybridCache cache) : IBasketRepository
{
    private readonly HybridCache _cache = cache;
    private readonly IBasketRepository _basketRepository = basketRepository;
    
    private const string Key = "baskets";

    public async Task<ShoppingCart?> GetBasketById(Guid id, CancellationToken cancellationToken = default)
    {
        var basketKey = $"{Key}_{id.ToString()}";
        
        var cachedBasket = await _cache.GetOrCreateAsync(basketKey, async token =>
            {
                var basket = await _basketRepository.GetBasketById(id, token);

                return JsonConvert.SerializeObject(basket);
                
            },
            tags: [Key],
            cancellationToken: cancellationToken);

        return JsonConvert.DeserializeObject<ShoppingCart>(cachedBasket);
        
    }

    public async Task StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        await _basketRepository.StoreBasket(basket, cancellationToken);
    }

    public async Task DeleteBasketById(Guid id, CancellationToken cancellationToken = default)
    {
        await _basketRepository.DeleteBasketById(id, cancellationToken);
        
        var basketKey = $"{Key}_{id.ToString()}";

        await _cache.RemoveAsync(basketKey, cancellationToken);
    }
}
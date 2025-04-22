using Basket.API.Entities;
using Marten;

namespace Basket.API.Data;

public class BasketRepository(IDocumentSession session) : IBasketRepository
{
    private readonly IDocumentSession _session = session;
    
    public async Task<ShoppingCart?> GetBasketById(Guid id, CancellationToken cancellationToken = default)
    {
        return await _session.LoadAsync<ShoppingCart>(id, cancellationToken);
    }

    public async Task StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        _session.Store(basket);
            
        await _session.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteBasketById(Guid id, CancellationToken cancellationToken = default)
    {
        _session.Delete(id);
            
        await _session.SaveChangesAsync(cancellationToken);
        
    }
}
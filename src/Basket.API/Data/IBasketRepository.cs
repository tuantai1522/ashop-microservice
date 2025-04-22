using Basket.API.Entities;

namespace Basket.API.Data;

public interface IBasketRepository
{
    /// <summary>
    /// Retrieves the shopping cart for the specified user.
    /// </summary>
    /// <param name="id">ID of user.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the shopping cart of the user.</returns>
    Task<ShoppingCart?> GetBasketById(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// To create the shopping cart for a user.
    /// </summary>
    /// <param name="basket">The shopping cart to be stored.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default);

    /// <summary>
    /// To delete the shopping cart for the specified user.
    /// </summary>
    /// <param name="id">Id of basket</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the deletion was successful.</returns>
    Task DeleteBasketById(Guid id, CancellationToken cancellationToken = default);

}
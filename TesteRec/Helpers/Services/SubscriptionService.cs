using Plugin.InAppBilling;
using System;
using System.Threading.Tasks;

public class SubscriptionService
{
    private static SubscriptionService _instance;
    private readonly IInAppBilling _billing;

    // Singleton pattern to ensure only one instance
    public static SubscriptionService Instance => _instance ??= new SubscriptionService();

    private SubscriptionService()
    {
        _billing = CrossInAppBilling.Current;
    }

    /// <summary>
    /// Initializes the billing service by connecting to the store.
    /// </summary>
    /// <returns>True if connected successfully, otherwise false.</returns>
    public async Task<bool> InitializeBillingAsync()
    {
        try
        {
            var connected = await _billing.ConnectAsync();
            return connected;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to billing service: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Purchases a subscription with the given product ID.
    /// </summary>
    /// <param name="productId">The product ID of the subscription to purchase.</param>
    /// <returns>True if the purchase was successful, otherwise false.</returns>
    public async Task<bool> PurchaseSubscriptionAsync(string productId)
    {
        try
        {
            var purchase = await _billing.PurchaseAsync(productId, ItemType.Subscription);
            return purchase != null;
        }
        catch (InAppBillingPurchaseException purchaseEx)
        {
            Console.WriteLine($"Purchase exception: {purchaseEx.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General exception: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Checks if the user has an active subscription for the given product ID.
    /// </summary>
    /// <param name="productId">The product ID of the subscription to check.</param>
    /// <returns>True if the subscription is active, otherwise false.</returns>
    //public async Task<bool> IsSubscriptionActiveAsync(string productId)
    //{
    //    try
    //    {
    //        var purchase = await _billing.GetPurchaseAsync(productId, ItemType.Subscription);
    //        return purchase != null && purchase.State == PurchaseState.Purchased;
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Error checking subscription: {ex.Message}");
    //        return false;
    //    }
    //}

    /// <summary>
    /// Finalizes a subscription purchase if required.
    /// </summary>
    /// <param name="purchaseTokens">An array of purchase tokens to finalize.</param>
    /// <returns>Task representing the finalization operation.</returns>
    public async Task FinalizePurchaseAsync(string[] purchaseTokens)
    {
        try
        {
            await _billing.FinalizePurchaseAsync(purchaseTokens);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error finalizing purchase: {ex.Message}");
        }
    }

    /// <summary>
    /// Disconnects the billing service.
    /// </summary>
    /// <returns>Task representing the disconnection operation.</returns>
    public async Task DisconnectBillingAsync()
    {
        try
        {
            await _billing.DisconnectAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error disconnecting from billing service: {ex.Message}");
        }
    }
}

using FarmerFactory.Repositories.Models;

namespace FarmerFactory.Test.Builders;

public class PurchaseBuilder
{
    private static readonly Random _random = new();

    private int _id;
    private int _clientId;
    private int _productId;
    private int _qty;
    private DateTime _purchaseTime;
    private Product _product;
    private Client _client;

    private PurchaseBuilder()
    {
        _id = _random.Next(1, 1000);
        _clientId = _random.Next(1, 1000);
        _productId = _random.Next(1, 1000);
        _qty = 1;
        _purchaseTime = DateTime.UtcNow;
        _product = null;
        _client = null;
    }

    public static PurchaseBuilder Builder()
    {
        return new PurchaseBuilder();
    }

    public Purchase Build()
    {
        return new Purchase
        {
            Id = _id,
            ClientId = _clientId,
            ProductId = _productId,
            Qty = _qty,
            PurchaseTime = _purchaseTime,
            Product = _product,
            Client = _client
        };
    }

    public PurchaseBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public PurchaseBuilder WithProductId(int productId)
    {
        _productId = productId;
        return this;
    }

    public PurchaseBuilder WithProduct(Product product)
    {
        _product = product;
        return this;
    }

    public PurchaseBuilder WithClient(Client client)
    {
        _client = client;
        return this;
    }

    public PurchaseBuilder WithClientId(int clientId)
    {
        _clientId = clientId;
        return this;
    }

    public PurchaseBuilder WithQty(int qty)
    {
        _qty = qty;
        return this;
    }

    public PurchaseBuilder WithPurchaseTime(DateTime purchaseTime)
    {
        _purchaseTime = purchaseTime;
        return this;
    }
}

using FarmerFactory.Repositories.Models;

namespace FarmerFactory.Test.Builders;

public class ProductBuilder
{
    private static readonly Random _random = new();

    private int _id;
    private string _name;
    private string? _description;
    private decimal _price;
    private int _stock;
    private int _productTypeId;
    private ProductType _productType;

    private ProductBuilder()
    {
        _id = _random.Next(1, 100);
        _name = "Test";
        _description = null;
        _price = 10.00m;
        _stock = 100;
        _productTypeId = 1;
        _productType = null;
    }

    public static ProductBuilder Builder()
    {
        return new ProductBuilder();
    }

    public Product Build()
    {
        return new Product
        {
            Id = _id,
            Name = _name,
            Description = _description,
            Price = _price,
            Stock = _stock,
            ProductTypeId = _productTypeId,
            ProductType = _productType
        };
    }

    public ProductBuilder WithId(int id)
    {
        _id = id;
        return this;
    }



    public ProductBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }





    public ProductBuilder WithProductTypeId(int productTypeId)
    {
        _productTypeId = productTypeId;
        return this;
    }

    public ProductBuilder WithProductType(ProductType productType)
    {
        _productType = productType;
        return this;
    }

    public ProductBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProductBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }

    public ProductBuilder WithStock(int stock)
    {
        _stock = stock;
        return this;
    }
}

using FarmerFactory.Repositories.Models;

namespace FarmerFactory.Test.Builders;

public class ProductTypeBuilder
{
    private static readonly Random _random = new();

    private int _id;
    private string _name;
    
    private ProductTypeBuilder()
    {
        _id = _random.Next(1, 100);
        _name = "Test";
    }

    public static ProductTypeBuilder Builder()
    {
        return new ProductTypeBuilder();
    }

    public ProductType Build()
    {
        return new ProductType
        {
            Id = _id,
            Name = _name
        };
    }

    public ProductTypeBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public ProductTypeBuilder WithName(string name)
    {
        _name = name;
        return this;
    }
}

using FarmerFactory.Repositories.Models;

namespace FarmerFactory.Test.Builders;

public class ClientBuilder
{
    private static readonly Random _random = new();

    private int _id;
    private string _name;
    
    private ClientBuilder()
    {
        _id = _random.Next(1, 100);
        _name = "Test";
    }

    public static ClientBuilder Builder()
    {
        return new ClientBuilder();
    }

    public Client Build()
    {
        return new Client
        {
            Id = _id,
            Name = _name
        };
    }

    public ClientBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public ClientBuilder WithName(string name)
    {
        _name = name;
        return this;
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmerFactory.Common.Entities.Product;

public class ProductResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public string ProductType { get; set; } = string.Empty;
}

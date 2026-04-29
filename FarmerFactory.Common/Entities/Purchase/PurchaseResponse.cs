using System.Security.Principal;

namespace FarmerFactory.Common.Entities.Purchase;

public class PurchaseResponse
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public int ProductId { get; set; }

    public int Qty { get; set; }

    public string ClientName { get; set; } = string.Empty;

    public string ProductName { get; set; } = string.Empty;

    public DateTime PurchaseDate { get; set; }
}

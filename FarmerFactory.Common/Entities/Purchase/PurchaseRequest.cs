using System.ComponentModel.DataAnnotations;

namespace FarmerFactory.Common.Entities.Purchase;

public class PurchaseRequest
{
    [Required]
    public int ClientId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public int Qty { get; set; }
}

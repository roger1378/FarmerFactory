using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmerFactory.Repositories.Models;

public class Purchase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(Client))]
    public int ClientId { get; set; }

    [Required]
    [ForeignKey(nameof(Product))]
    public int ProductId { get; set; }

    public int Qty { get; set; }

    [Required]
    public DateTime PurchaseTime { get; set; } = DateTime.UtcNow;

    public virtual Product Product { get; set; }

    public virtual Client Client { get; set; }
}

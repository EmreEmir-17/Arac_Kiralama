using AracKiralama.Data.Enums;

namespace AracKiralama.Data.Entities;

public class Invoice
{
    public int Id { get; set; }
    public int RentalId { get; set; }
    public Rental Rental { get; set; } = null!;

    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime IssueDateUtc { get; set; }
    public decimal SubTotal { get; set; }
    public decimal TaxRate { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal Total { get; set; }
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

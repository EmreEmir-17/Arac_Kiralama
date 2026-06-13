using AracKiralama.Data.Enums;

namespace AracKiralama.Data.Entities;

public class Payment
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public Invoice Invoice { get; set; } = null!;
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public DateTime PaidUtc { get; set; }
    public string? ReferenceNote { get; set; }
}

using AracKiralama.Data.Enums;

namespace AracKiralama.Data.Entities;

public class Rental
{
    public int Id { get; set; }
    public int? ReservationId { get; set; }
    public Reservation? Reservation { get; set; }

    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public int? CampaignId { get; set; }
    public Campaign? Campaign { get; set; }

    public DateTime StartUtc { get; set; }
    public DateTime EndUtc { get; set; }
    public RentalState State { get; set; } = RentalState.Confirmed;
    public decimal? TotalPrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public string? Notes { get; set; }

    public ICollection<RentalExtraLine> ExtraLines { get; set; } = new List<RentalExtraLine>();
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    public ICollection<ContractDocument> ContractDocuments { get; set; } = new List<ContractDocument>();
}

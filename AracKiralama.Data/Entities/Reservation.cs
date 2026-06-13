using AracKiralama.Data.Enums;

namespace AracKiralama.Data.Entities;

/// <summary>Ön rezervasyon; onaylanınca kiralama (sözleşme) kaydına dönüştürülür.</summary>
public class Reservation
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;

    public DateTime StartUtc { get; set; }
    public DateTime EndUtc { get; set; }
    public ReservationState State { get; set; } = ReservationState.Pending;
    public decimal? EstimatedTotal { get; set; }
    public int? CampaignId { get; set; }
    public Campaign? Campaign { get; set; }
    public string? Notes { get; set; }

    public ICollection<ReservationExtraLine> ExtraLines { get; set; } = new List<ReservationExtraLine>();

    public Rental? Rental { get; set; }
}

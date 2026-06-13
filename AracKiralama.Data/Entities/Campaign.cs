namespace AracKiralama.Data.Entities;

public class Campaign
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal? PercentDiscount { get; set; }
    public decimal? FixedDiscount { get; set; }
    public DateTime ValidFromUtc { get; set; }
    public DateTime ValidToUtc { get; set; }
    public bool IsActive { get; set; } = true;
    public int? MaxRedemptions { get; set; }
    public int RedeemedCount { get; set; }

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}

namespace AracKiralama.Data.Entities;

public class DamageReport
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
    public int? RentalId { get; set; }
    public Rental? Rental { get; set; }

    public DateTime ReportedUtc { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Severity { get; set; }
    public string? PhotoRelativePath { get; set; }
    public bool Resolved { get; set; }
}

namespace AracKiralama.Data.Entities;

public class MaintenanceRecord
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;

    public DateTime ServiceDateUtc { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal? Cost { get; set; }
    public int? OdometerKm { get; set; }
}

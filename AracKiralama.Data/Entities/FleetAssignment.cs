namespace AracKiralama.Data.Entities;

public class FleetAssignment
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;

    /// <summary>Çalışan adı veya departman ünvanı.</summary>
    public string TargetName { get; set; } = string.Empty;

    public DateTime StartUtc { get; set; }
    public DateTime? EndUtc { get; set; }
    public string? Notes { get; set; }
}

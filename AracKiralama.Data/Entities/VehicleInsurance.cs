namespace AracKiralama.Data.Entities;

public class VehicleInsurance
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
    public string PolicyNumber { get; set; } = string.Empty;
    public string Insurer { get; set; } = string.Empty;
    public DateTime StartDateUtc { get; set; }
    public DateTime EndDateUtc { get; set; }
    public string? Notes { get; set; }
    public string? DocumentRelativePath { get; set; }
}

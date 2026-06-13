namespace AracKiralama.Data.Entities;

/// <summary>Muayene kaydı.</summary>
public class VehicleInspection
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
    public DateTime InspectionDateUtc { get; set; }
    public DateTime ValidUntilUtc { get; set; }
    public string? Station { get; set; }
    public string? DocumentRelativePath { get; set; }
}

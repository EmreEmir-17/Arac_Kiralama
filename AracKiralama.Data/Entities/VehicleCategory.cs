namespace AracKiralama.Data.Entities;

public class VehicleCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Code { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    public ICollection<SeasonalPriceRule> SeasonalPriceRules { get; set; } = new List<SeasonalPriceRule>();
}

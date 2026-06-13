namespace AracKiralama.Data.Entities;

/// <summary>
/// Belirli tarih aralığında günlük ücreti geçersiz kılar. VehicleId doluysa araç özel, değilse kategori geneli.
/// </summary>
public class SeasonalPriceRule
{
    public int Id { get; set; }
    public int? VehicleCategoryId { get; set; }
    public VehicleCategory? VehicleCategory { get; set; }
    public int? VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }

    public DateTime ValidFromUtc { get; set; }
    public DateTime ValidToUtc { get; set; }
    public decimal DailyRate { get; set; }
}

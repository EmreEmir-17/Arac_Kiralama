namespace AracKiralama.Data.Entities;

public class ExtraProduct
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<RentalExtraLine> RentalExtraLines { get; set; } = new List<RentalExtraLine>();

    public ICollection<ReservationExtraLine> ReservationExtraLines { get; set; } = new List<ReservationExtraLine>();
}

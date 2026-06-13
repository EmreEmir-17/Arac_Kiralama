namespace AracKiralama.Data.Entities;

public class ReservationExtraLine
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public Reservation Reservation { get; set; } = null!;
    public int ExtraProductId { get; set; }
    public ExtraProduct ExtraProduct { get; set; } = null!;
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
}

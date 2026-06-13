using AracKiralama.Data;

namespace AracKiralama.Data.Entities;

public class Customer
{
    public int Id { get; set; }
    public string? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }

    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? TaxNumber { get; set; }
    public bool IsCorporate { get; set; }

    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}

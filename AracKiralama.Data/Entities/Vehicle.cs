using AracKiralama.Data.Enums;

namespace AracKiralama.Data.Entities;

public class Vehicle
{
    public int Id { get; set; }
    public int BranchId { get; set; }
    public Branch Branch { get; set; } = null!;

    public int VehicleCategoryId { get; set; }
    public VehicleCategory VehicleCategory { get; set; } = null!;

    public string Plate { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int? ModelYear { get; set; }
    /// <summary>Gösterge kilometresi (liste ve müşteri detayı).</summary>
    public int? OdometerKm { get; set; }
    /// <summary>Koltuk sayısı.</summary>
    public int? Seats { get; set; }
    public string? FuelType { get; set; }
    public string? Transmission { get; set; }
    /// <summary>Donanım / özellik metni (klima, navigasyon vb.).</summary>
    public string? Features { get; set; }
    public decimal DailyRate { get; set; }
    public VehicleStatus Status { get; set; } = VehicleStatus.Available;

    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    public ICollection<FleetAssignment> FleetAssignments { get; set; } = new List<FleetAssignment>();
    public ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } = new List<MaintenanceRecord>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public ICollection<VehicleInsurance> Insurances { get; set; } = new List<VehicleInsurance>();
    public ICollection<VehicleInspection> Inspections { get; set; } = new List<VehicleInspection>();
    public ICollection<DamageReport> DamageReports { get; set; } = new List<DamageReport>();
}

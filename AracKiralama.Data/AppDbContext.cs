using AracKiralama.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AracKiralama.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<VehicleCategory> VehicleCategories => Set<VehicleCategory>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Rental> Rentals => Set<Rental>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<Campaign> Campaigns => Set<Campaign>();
    public DbSet<SeasonalPriceRule> SeasonalPriceRules => Set<SeasonalPriceRule>();
    public DbSet<ExtraProduct> ExtraProducts => Set<ExtraProduct>();
    public DbSet<RentalExtraLine> RentalExtraLines => Set<RentalExtraLine>();
    public DbSet<ReservationExtraLine> ReservationExtraLines => Set<ReservationExtraLine>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<DamageReport> DamageReports => Set<DamageReport>();
    public DbSet<ContractDocument> ContractDocuments => Set<ContractDocument>();
    public DbSet<VehicleInsurance> VehicleInsurances => Set<VehicleInsurance>();
    public DbSet<VehicleInspection> VehicleInspections => Set<VehicleInspection>();
    public DbSet<StoredDocument> StoredDocuments => Set<StoredDocument>();
    public DbSet<FleetAssignment> FleetAssignments => Set<FleetAssignment>();
    public DbSet<MaintenanceRecord> MaintenanceRecords => Set<MaintenanceRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>(e =>
        {
            e.HasOne(x => x.LinkedCustomer)
                .WithOne(x => x.ApplicationUser)
                .HasForeignKey<Customer>(x => x.ApplicationUserId)
                .IsRequired(false);
        });

        modelBuilder.Entity<Branch>(e =>
        {
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
            e.Property(x => x.City).HasMaxLength(100);
            e.Property(x => x.AddressLine).HasMaxLength(500);
            e.Property(x => x.Phone).HasMaxLength(50);
        });

        modelBuilder.Entity<VehicleCategory>(e =>
        {
            e.Property(x => x.Name).HasMaxLength(120).IsRequired();
            e.Property(x => x.Description).HasMaxLength(500);
            e.Property(x => x.Code).HasMaxLength(32);
        });

        modelBuilder.Entity<Vehicle>(e =>
        {
            e.HasIndex(x => x.Plate).IsUnique();
            e.Property(x => x.Plate).HasMaxLength(32).IsRequired();
            e.Property(x => x.Brand).HasMaxLength(100).IsRequired();
            e.Property(x => x.Model).HasMaxLength(100).IsRequired();
            e.Property(x => x.FuelType).HasMaxLength(64);
            e.Property(x => x.Transmission).HasMaxLength(64);
            e.Property(x => x.Features).HasMaxLength(4000);
            e.Property(x => x.DailyRate).HasPrecision(18, 2);
            e.HasOne(x => x.Branch).WithMany(x => x.Vehicles).HasForeignKey(x => x.BranchId);
            e.HasOne(x => x.VehicleCategory).WithMany(x => x.Vehicles).HasForeignKey(x => x.VehicleCategoryId);
        });

        modelBuilder.Entity<Customer>(e =>
        {
            e.Property(x => x.FullName).HasMaxLength(200).IsRequired();
            e.Property(x => x.Email).HasMaxLength(256);
            e.Property(x => x.Phone).HasMaxLength(50);
            e.Property(x => x.TaxNumber).HasMaxLength(32);
            e.HasIndex(x => x.ApplicationUserId).IsUnique().HasFilter("[ApplicationUserId] IS NOT NULL");
        });

        modelBuilder.Entity<Campaign>(e =>
        {
            e.Property(x => x.Code).HasMaxLength(40).IsRequired();
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
            e.Property(x => x.PercentDiscount).HasPrecision(9, 4);
            e.Property(x => x.FixedDiscount).HasPrecision(18, 2);
            e.HasIndex(x => x.Code).IsUnique();
        });

        modelBuilder.Entity<SeasonalPriceRule>(e =>
        {
            e.Property(x => x.DailyRate).HasPrecision(18, 2);
            e.HasOne(x => x.VehicleCategory).WithMany(x => x.SeasonalPriceRules).HasForeignKey(x => x.VehicleCategoryId).IsRequired(false);
            e.HasOne(x => x.Vehicle).WithMany().HasForeignKey(x => x.VehicleId).IsRequired(false);
        });

        modelBuilder.Entity<ExtraProduct>(e =>
        {
            e.Property(x => x.Code).HasMaxLength(40).IsRequired();
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
            e.Property(x => x.UnitPrice).HasPrecision(18, 2);
            e.HasIndex(x => x.Code).IsUnique();
        });

        modelBuilder.Entity<Reservation>(e =>
        {
            e.Property(x => x.Notes).HasMaxLength(2000);
            e.Property(x => x.EstimatedTotal).HasPrecision(18, 2);
            e.HasOne(x => x.Customer).WithMany(x => x.Reservations).HasForeignKey(x => x.CustomerId);
            e.HasOne(x => x.Vehicle).WithMany(x => x.Reservations).HasForeignKey(x => x.VehicleId);
            e.HasOne(x => x.Campaign).WithMany(x => x.Reservations).HasForeignKey(x => x.CampaignId).IsRequired(false);
        });

        modelBuilder.Entity<ReservationExtraLine>(e =>
        {
            e.Property(x => x.UnitPrice).HasPrecision(18, 2);
            e.Property(x => x.LineTotal).HasPrecision(18, 2);
            e.HasOne(x => x.Reservation).WithMany(x => x.ExtraLines).HasForeignKey(x => x.ReservationId);
            e.HasOne(x => x.ExtraProduct).WithMany(x => x.ReservationExtraLines).HasForeignKey(x => x.ExtraProductId);
        });

        modelBuilder.Entity<Rental>(e =>
        {
            e.Property(x => x.TotalPrice).HasPrecision(18, 2);
            e.Property(x => x.DiscountAmount).HasPrecision(18, 2);
            e.Property(x => x.Notes).HasMaxLength(2000);
            e.HasIndex(x => x.ReservationId).IsUnique().HasFilter("[ReservationId] IS NOT NULL");
            e.HasOne(x => x.Vehicle).WithMany(x => x.Rentals).HasForeignKey(x => x.VehicleId);
            e.HasOne(x => x.Customer).WithMany(x => x.Rentals).HasForeignKey(x => x.CustomerId);
            e.HasOne(x => x.Campaign).WithMany(x => x.Rentals).HasForeignKey(x => x.CampaignId).IsRequired(false);
            e.HasOne(x => x.Reservation).WithOne(x => x.Rental).HasForeignKey<Rental>(x => x.ReservationId).IsRequired(false);
        });

        modelBuilder.Entity<RentalExtraLine>(e =>
        {
            e.Property(x => x.UnitPrice).HasPrecision(18, 2);
            e.Property(x => x.LineTotal).HasPrecision(18, 2);
            e.HasOne(x => x.Rental).WithMany(x => x.ExtraLines).HasForeignKey(x => x.RentalId);
            e.HasOne(x => x.ExtraProduct).WithMany(x => x.RentalExtraLines).HasForeignKey(x => x.ExtraProductId);
        });

        modelBuilder.Entity<Invoice>(e =>
        {
            e.Property(x => x.InvoiceNumber).HasMaxLength(40).IsRequired();
            e.Property(x => x.SubTotal).HasPrecision(18, 2);
            e.Property(x => x.TaxRate).HasPrecision(9, 4);
            e.Property(x => x.TaxAmount).HasPrecision(18, 2);
            e.Property(x => x.Total).HasPrecision(18, 2);
            e.HasIndex(x => x.InvoiceNumber).IsUnique();
            e.HasIndex(x => x.RentalId).IsUnique();
            e.HasOne(x => x.Rental).WithMany(x => x.Invoices).HasForeignKey(x => x.RentalId);
        });

        modelBuilder.Entity<Payment>(e =>
        {
            e.Property(x => x.Amount).HasPrecision(18, 2);
            e.Property(x => x.ReferenceNote).HasMaxLength(500);
            e.HasOne(x => x.Invoice).WithMany(x => x.Payments).HasForeignKey(x => x.InvoiceId);
        });

        modelBuilder.Entity<DamageReport>(e =>
        {
            e.Property(x => x.Description).HasMaxLength(4000).IsRequired();
            e.Property(x => x.Severity).HasMaxLength(50);
            e.Property(x => x.PhotoRelativePath).HasMaxLength(500);
            e.HasOne(x => x.Vehicle).WithMany(x => x.DamageReports).HasForeignKey(x => x.VehicleId);
            e.HasOne(x => x.Rental).WithMany().HasForeignKey(x => x.RentalId).IsRequired(false);
        });

        modelBuilder.Entity<ContractDocument>(e =>
        {
            e.Property(x => x.PdfRelativePath).HasMaxLength(500).IsRequired();
            e.Property(x => x.TemplateVersion).HasMaxLength(20).IsRequired();
            e.HasOne(x => x.Rental).WithMany(x => x.ContractDocuments).HasForeignKey(x => x.RentalId);
        });

        modelBuilder.Entity<VehicleInsurance>(e =>
        {
            e.Property(x => x.PolicyNumber).HasMaxLength(100).IsRequired();
            e.Property(x => x.Insurer).HasMaxLength(200).IsRequired();
            e.Property(x => x.Notes).HasMaxLength(2000);
            e.Property(x => x.DocumentRelativePath).HasMaxLength(500);
            e.HasOne(x => x.Vehicle).WithMany(x => x.Insurances).HasForeignKey(x => x.VehicleId);
        });

        modelBuilder.Entity<VehicleInspection>(e =>
        {
            e.Property(x => x.Station).HasMaxLength(200);
            e.Property(x => x.DocumentRelativePath).HasMaxLength(500);
            e.HasOne(x => x.Vehicle).WithMany(x => x.Inspections).HasForeignKey(x => x.VehicleId);
        });

        modelBuilder.Entity<StoredDocument>(e =>
        {
            e.Property(x => x.FileName).HasMaxLength(260).IsRequired();
            e.Property(x => x.RelativePath).HasMaxLength(500).IsRequired();
            e.HasIndex(x => new { x.EntityKind, x.EntityId });
        });

        modelBuilder.Entity<FleetAssignment>(e =>
        {
            e.Property(x => x.TargetName).HasMaxLength(200).IsRequired();
            e.Property(x => x.Notes).HasMaxLength(2000);
            e.HasOne(x => x.Vehicle).WithMany(x => x.FleetAssignments).HasForeignKey(x => x.VehicleId);
        });

        modelBuilder.Entity<MaintenanceRecord>(e =>
        {
            e.Property(x => x.Description).HasMaxLength(2000).IsRequired();
            e.Property(x => x.Cost).HasPrecision(18, 2);
            e.HasOne(x => x.Vehicle).WithMany(x => x.MaintenanceRecords).HasForeignKey(x => x.VehicleId);
        });
    }
}

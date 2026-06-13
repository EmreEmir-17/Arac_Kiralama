using AracKiralama.Data;
using AracKiralama.Data.Entities;
using AracKiralama.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AracKiralama.Web.Auth;

/// <summary>
/// Örnek şube, araç, müşteri, sezon fiyatı, rezervasyon vb. — yalnızca Seed:DemoData=true iken ve bir kez çalışır.
/// </summary>
public static class DemoDataSeeder
{
    private const string DemoPlateMarker = "34 DEMO 01";
    private const string DemoBillingMarker = "DEMO-BILLING-SEED";

    public static async Task SeedIfEnabledAsync(IServiceProvider services, IConfiguration configuration)
    {
        if (!configuration.GetValue("Seed:DemoData", false))
        {
            return;
        }

        var db = services.GetRequiredService<AppDbContext>();
        if (await db.Vehicles.AnyAsync(v => v.Plate == DemoPlateMarker))
        {
            return;
        }

        var merkez = new Branch
        {
            Name = "Merkez Ofis",
            City = "İstanbul",
            AddressLine = "Maslak Mah. Örnek Cad. No:1",
            Phone = "+90 212 555 0101"
        };

        var havalimani = new Branch
        {
            Name = "İstanbul Havalimanı",
            City = "İstanbul",
            AddressLine = "Terminal karşısı",
            Phone = "+90 212 555 0102"
        };

        db.Branches.AddRange(merkez, havalimani);
        await db.SaveChangesAsync();

        var vEgea = new Vehicle
        {
            BranchId = merkez.Id,
            VehicleCategoryId = 1,
            Plate = DemoPlateMarker,
            Brand = "Fiat",
            Model = "Egea",
            ModelYear = 2023,
            OdometerKm = 42_000,
            Seats = 5,
            FuelType = "Dizel",
            Transmission = "Manuel",
            Features = "Klima, cruise control, park sensörü (arka).",
            DailyRate = 1200,
            Status = VehicleStatus.Available
        };

        var vDuster = new Vehicle
        {
            BranchId = merkez.Id,
            VehicleCategoryId = 2,
            Plate = "34 DEMO 02",
            Brand = "Dacia",
            Model = "Duster",
            ModelYear = 2022,
            OdometerKm = 58_200,
            Seats = 5,
            FuelType = "Benzin",
            Transmission = "Manuel",
            Features = "4x4, roof bar, dijital gösterge.",
            DailyRate = 1450,
            Status = VehicleStatus.Available
        };

        var vTucson = new Vehicle
        {
            BranchId = havalimani.Id,
            VehicleCategoryId = 2,
            Plate = "34 DEMO 03",
            Brand = "Hyundai",
            Model = "Tucson",
            ModelYear = 2024,
            OdometerKm = 12_500,
            Seats = 5,
            FuelType = "Hybrid",
            Transmission = "Otomatik",
            Features = "Adaptif cruise, şerit takip, CarPlay / Android Auto, deri döşeme.",
            DailyRate = 2100,
            Status = VehicleStatus.Available
        };

        var vPassat = new Vehicle
        {
            BranchId = havalimani.Id,
            VehicleCategoryId = 3,
            Plate = "34 DEMO 04",
            Brand = "Volkswagen",
            Model = "Passat",
            ModelYear = 2023,
            OdometerKm = 33_000,
            Seats = 5,
            FuelType = "Dizel",
            Transmission = "Otomatik",
            Features = "Matrix LED, ergoComfort koltuk, head-up display.",
            DailyRate = 2800,
            Status = VehicleStatus.Available
        };

        var vClio = new Vehicle
        {
            BranchId = merkez.Id,
            VehicleCategoryId = 1,
            Plate = "34 DEMO 05",
            Brand = "Renault",
            Model = "Clio",
            ModelYear = 2021,
            OdometerKm = 71_000,
            Seats = 5,
            FuelType = "Benzin",
            Transmission = "Otomatik",
            Features = "Easy Park, ısıtmalı direksiyon.",
            DailyRate = 950,
            Status = VehicleStatus.Maintenance
        };

        db.Vehicles.AddRange(vEgea, vDuster, vTucson, vPassat, vClio);
        await db.SaveChangesAsync();

        var ayse = new Customer
        {
            FullName = "Ayşe Yılmaz",
            Email = "ayse.demo@local",
            Phone = "+90 555 111 2233",
            IsCorporate = false
        };

        var mehmet = new Customer
        {
            FullName = "Mehmet Kaya",
            Email = "mehmet.demo@local",
            Phone = "+90 555 444 5566",
            IsCorporate = false
        };

        var kurumsal = new Customer
        {
            FullName = "ABC Lojistik A.Ş.",
            Email = "filo@abc-demo.local",
            Phone = "+90 216 555 7788",
            TaxNumber = "1234567890",
            IsCorporate = true
        };

        db.Customers.AddRange(ayse, mehmet, kurumsal);
        await db.SaveChangesAsync();

        var welcome = await db.Campaigns.AsNoTracking().FirstOrDefaultAsync(c => c.Code == "WELCOME10");
        var day = DateTime.UtcNow.AddDays(10);
        var startUtc = new DateTime(day.Year, day.Month, day.Day, 6, 0, 0, DateTimeKind.Utc);
        var endUtc = startUtc.AddDays(4);

        db.Reservations.Add(new Reservation
        {
            CustomerId = ayse.Id,
            VehicleId = vTucson.Id,
            StartUtc = startUtc,
            EndUtc = endUtc,
            State = ReservationState.Pending,
            EstimatedTotal = 8400,
            CampaignId = welcome?.Id,
            Notes = "Demo: havalimanı teslim talebi."
        });

        var y = DateTime.UtcNow.Year;
        var summerStart = new DateTime(y, 6, 1, 0, 0, 0, DateTimeKind.Utc);
        var summerEnd = new DateTime(y, 8, 31, 23, 59, 59, DateTimeKind.Utc);
        db.SeasonalPriceRules.Add(new SeasonalPriceRule
        {
            VehicleCategoryId = 2,
            VehicleId = null,
            ValidFromUtc = summerStart,
            ValidToUtc = summerEnd,
            DailyRate = 3200
        });

        db.SeasonalPriceRules.Add(new SeasonalPriceRule
        {
            VehicleCategoryId = null,
            VehicleId = vPassat.Id,
            ValidFromUtc = summerStart,
            ValidToUtc = summerEnd,
            DailyRate = 3500
        });

        db.VehicleInsurances.Add(new VehicleInsurance
        {
            VehicleId = vEgea.Id,
            PolicyNumber = "DEMO-POL-2026-001",
            Insurer = "Örnek Sigorta A.Ş.",
            StartDateUtc = DateTime.UtcNow.AddMonths(-2),
            EndDateUtc = DateTime.UtcNow.AddMonths(10),
            Notes = "Kasko + trafik (demo)."
        });

        db.VehicleInspections.Add(new VehicleInspection
        {
            VehicleId = vEgea.Id,
            InspectionDateUtc = DateTime.UtcNow.AddMonths(-1),
            ValidUntilUtc = DateTime.UtcNow.AddMonths(11),
            Station = "TÜVTÜRK Maslak"
        });

        db.FleetAssignments.Add(new FleetAssignment
        {
            VehicleId = vDuster.Id,
            TargetName = "ABC Lojistik — Satış ekibi",
            StartUtc = DateTime.UtcNow.AddDays(-30),
            EndUtc = null,
            Notes = "Demo filo ataması."
        });

        db.MaintenanceRecords.Add(new MaintenanceRecord
        {
            VehicleId = vClio.Id,
            ServiceDateUtc = DateTime.UtcNow.AddDays(-5),
            Description = "Periyodik bakım + fren kontrolü (demo).",
            Cost = 4500,
            OdometerKm = 48_200
        });

        await db.SaveChangesAsync();
    }

    /// <summary>
    /// Seed:DemoData=true iken: kiralama tutarlarını tamamlar, km boşluklarını doldurur,
    /// örnek kiralama / fatura / ödeme verilerini ve eski rezervasyon tahminlerini günceller (idempotent).
    /// </summary>
    public static async Task EnsureBillingAndDataSyncAsync(IServiceProvider services, IConfiguration configuration)
    {
        if (!configuration.GetValue("Seed:DemoData", false))
        {
            return;
        }

        var db = services.GetRequiredService<AppDbContext>();

        var rentalsNull = await db.Rentals.Include(r => r.ExtraLines).Where(r => r.TotalPrice == null).ToListAsync();
        foreach (var rental in rentalsNull)
        {
            var (_, disc, baseNet) = await PricingResolver.ComputeBaseRentalAsync(db, rental.VehicleId, rental.StartUtc, rental.EndUtc, rental.CampaignId);
            rental.TotalPrice = baseNet;
            rental.DiscountAmount = disc;
        }

        if (rentalsNull.Count > 0)
        {
            await db.SaveChangesAsync();
        }

        var vehKm = await db.Vehicles.Where(v => v.OdometerKm == null).ToListAsync();
        foreach (var v in vehKm)
        {
            v.OdometerKm = 35000 + v.Id * 791 % 85000;
        }

        if (vehKm.Count > 0)
        {
            await db.SaveChangesAsync();
        }

        if (!await db.Rentals.AnyAsync(r => r.Notes == DemoBillingMarker))
        {
            var ayse = await db.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Email == "ayse.demo@local");
            var mehmet = await db.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Email == "mehmet.demo@local");
            var kurumsal = await db.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Email == "filo@abc-demo.local");
            var vEgea = await db.Vehicles.AsNoTracking().FirstOrDefaultAsync(v => v.Plate == DemoPlateMarker);
            var vDuster = await db.Vehicles.AsNoTracking().FirstOrDefaultAsync(v => v.Plate == "34 DEMO 02");
            var vPassat = await db.Vehicles.AsNoTracking().FirstOrDefaultAsync(v => v.Plate == "34 DEMO 04");
            var gps = await db.ExtraProducts.AsNoTracking().FirstOrDefaultAsync(e => e.Code == "GPS");

            if (ayse is not null && vEgea is not null)
            {
                var r1 = new Rental
                {
                    CustomerId = ayse.Id,
                    VehicleId = vEgea.Id,
                    StartUtc = DateTime.UtcNow.AddDays(-2),
                    EndUtc = DateTime.UtcNow.AddDays(7),
                    State = RentalState.Confirmed,
                    Notes = DemoBillingMarker
                };
                db.Rentals.Add(r1);
                await db.SaveChangesAsync();
                if (gps is not null)
                {
                    db.RentalExtraLines.Add(new RentalExtraLine
                    {
                        RentalId = r1.Id,
                        ExtraProductId = gps.Id,
                        Quantity = 1,
                        UnitPrice = gps.UnitPrice,
                        LineTotal = gps.UnitPrice
                    });
                }
            }

            if (mehmet is not null && vDuster is not null)
            {
                db.Rentals.Add(new Rental
                {
                    CustomerId = mehmet.Id,
                    VehicleId = vDuster.Id,
                    StartUtc = DateTime.UtcNow.AddDays(-70),
                    EndUtc = DateTime.UtcNow.AddDays(-40),
                    State = RentalState.Completed,
                    Notes = DemoBillingMarker
                });
            }

            if (kurumsal is not null && vPassat is not null)
            {
                db.Rentals.Add(new Rental
                {
                    CustomerId = kurumsal.Id,
                    VehicleId = vPassat.Id,
                    StartUtc = DateTime.UtcNow.AddDays(14),
                    EndUtc = DateTime.UtcNow.AddDays(21),
                    State = RentalState.Confirmed,
                    Notes = DemoBillingMarker
                });
            }

            await db.SaveChangesAsync();
        }

        var demoRentalIds = await db.Rentals.AsNoTracking().Where(r => r.Notes == DemoBillingMarker).Select(r => r.Id).ToListAsync();
        foreach (var rid in demoRentalIds)
        {
            await InvoiceBilling.CreateInvoiceForRentalIfMissingAsync(db, rid);
        }

        var demoInvoices = await db.Invoices
            .Include(i => i.Payments)
            .Where(i => demoRentalIds.Contains(i.RentalId))
            .OrderBy(i => i.Id)
            .ToListAsync();
        var unpaidDemo = demoInvoices.Where(i => i.Payments.Count == 0).OrderBy(i => i.Id).ToList();
        var didPay = false;
        for (var i = 0; i < unpaidDemo.Count; i++)
        {
            var inv = unpaidDemo[i];
            var ordinal = i + 1;
            var amt = ordinal == 1 ? inv.Total : Math.Min(inv.Total, Math.Round(inv.Total * 0.45m, 2));
            if (amt <= 0.009m)
            {
                continue;
            }

            db.Payments.Add(new Payment
            {
                InvoiceId = inv.Id,
                Amount = amt,
                Method = ordinal == 1 ? PaymentMethod.Card : PaymentMethod.BankTransfer,
                PaidUtc = DateTime.UtcNow.AddMinutes(-ordinal * 13),
                ReferenceNote = ordinal == 1 ? "Demo tam ödeme" : "Demo kısmi ödeme"
            });
            if (amt >= inv.Total - 0.01m)
            {
                inv.Status = InvoiceStatus.Paid;
            }

            didPay = true;
        }

        if (didPay)
        {
            await db.SaveChangesAsync();
        }

        await SyncReservationEstimatesAsync(db);
    }

    private static async Task SyncReservationEstimatesAsync(AppDbContext db)
    {
        var list = await db.Reservations
            .Include(r => r.ExtraLines)
            .Where(r => r.State == ReservationState.Pending || r.State == ReservationState.Confirmed)
            .ToListAsync();
        var changed = false;
        foreach (var res in list)
        {
            if (res.EstimatedTotal is > 0m)
            {
                continue;
            }

            var (_, _, baseNet) = await PricingResolver.ComputeBaseRentalAsync(db, res.VehicleId, res.StartUtc, res.EndUtc, res.CampaignId);
            var extras = res.ExtraLines.Sum(x => x.LineTotal);
            res.EstimatedTotal = baseNet + extras;
            changed = true;
        }

        if (changed)
        {
            await db.SaveChangesAsync();
        }
    }
}

using AracKiralama.Data.Entities;
using AracKiralama.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AracKiralama.Data;

/// <summary>Kiralama için fatura oluşturma (personel ekranı ve tohum veri).</summary>
public static class InvoiceBilling
{
    /// <summary>Kiralama için fatura yoksa oluşturur; kiralama tutarını günceller.</summary>
    /// <returns>Oluşturulan fatura veya zaten varsa / kiralama yoksa null.</returns>
    public static async Task<Invoice?> CreateInvoiceForRentalIfMissingAsync(
        AppDbContext db,
        int rentalId,
        CancellationToken cancellationToken = default)
    {
        if (await db.Invoices.AnyAsync(i => i.RentalId == rentalId, cancellationToken))
        {
            return null;
        }

        var rental = await db.Rentals.Include(r => r.ExtraLines).FirstOrDefaultAsync(r => r.Id == rentalId, cancellationToken);
        if (rental is null)
        {
            return null;
        }

        var extras = rental.ExtraLines.Sum(x => x.LineTotal);
        var (_, disc, baseNet) = await PricingResolver.ComputeBaseRentalAsync(db, rental.VehicleId, rental.StartUtc, rental.EndUtc, rental.CampaignId, cancellationToken);
        rental.TotalPrice = baseNet;
        rental.DiscountAmount = disc;
        var sub = baseNet + extras;
        const decimal taxRate = 0.20m;
        var tax = Math.Round(sub * taxRate, 2);
        var total = sub + tax;
        var year = DateTime.UtcNow.Year;
        var count = await db.Invoices.CountAsync(cancellationToken);
        var number = $"INV-{year}-{(count + 1):D5}";

        var inv = new Invoice
        {
            RentalId = rentalId,
            InvoiceNumber = number,
            IssueDateUtc = DateTime.UtcNow,
            SubTotal = sub,
            TaxRate = taxRate,
            TaxAmount = tax,
            Total = total,
            Status = InvoiceStatus.Issued
        };
        db.Invoices.Add(inv);
        await db.SaveChangesAsync(cancellationToken);
        return inv;
    }
}

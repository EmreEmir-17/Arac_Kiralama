using AracKiralama.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AracKiralama.Data;

public static class PricingResolver
{
    /// <summary>
    /// Önce araç özel sezon, yoksa kategori sezonu, yoksa aracın taban günlük ücreti.
    /// </summary>
    public static async Task<decimal> GetDailyRateAsync(
        AppDbContext db,
        int vehicleId,
        DateTime dayUtc,
        CancellationToken cancellationToken = default)
    {
        var v = await db.Vehicles.AsNoTracking()
            .Include(x => x.VehicleCategory)
            .FirstOrDefaultAsync(x => x.Id == vehicleId, cancellationToken);
        if (v is null)
        {
            return 0;
        }

        var vehicleRule = await db.SeasonalPriceRules.AsNoTracking()
            .Where(r => r.VehicleId == vehicleId && r.ValidFromUtc <= dayUtc && r.ValidToUtc >= dayUtc)
            .OrderByDescending(r => r.ValidFromUtc)
            .FirstOrDefaultAsync(cancellationToken);
        if (vehicleRule is not null)
        {
            return vehicleRule.DailyRate;
        }

        var catRule = await db.SeasonalPriceRules.AsNoTracking()
            .Where(r => r.VehicleCategoryId == v.VehicleCategoryId && r.VehicleId == null && r.ValidFromUtc <= dayUtc && r.ValidToUtc >= dayUtc)
            .OrderByDescending(r => r.ValidFromUtc)
            .FirstOrDefaultAsync(cancellationToken);
        if (catRule is not null)
        {
            return catRule.DailyRate;
        }

        return v.DailyRate;
    }

    public static decimal ApplyCampaign(decimal subTotal, Campaign? campaign)
    {
        if (campaign is null || !campaign.IsActive)
        {
            return 0;
        }

        var now = DateTime.UtcNow;
        if (now < campaign.ValidFromUtc || now > campaign.ValidToUtc)
        {
            return 0;
        }

        decimal discount = 0;
        if (campaign.PercentDiscount is { } p && p > 0)
        {
            discount += Math.Round(subTotal * (p / 100m), 2);
        }

        if (campaign.FixedDiscount is { } f && f > 0)
        {
            discount += f;
        }

        return Math.Min(discount, subTotal);
    }

    /// <summary>Günlük ücret × gün ve isteğe bağlı kampanya ile kiralama taban tutarı (ek ürünler hariç).</summary>
    public static async Task<(decimal SubBeforeDiscount, decimal DiscountAmount, decimal BaseNet)> ComputeBaseRentalAsync(
        AppDbContext db,
        int vehicleId,
        DateTime startUtc,
        DateTime endUtc,
        int? campaignId,
        CancellationToken cancellationToken = default)
    {
        var days = Math.Max(1, (decimal)(endUtc - startUtc).TotalDays);
        var daily = await GetDailyRateAsync(db, vehicleId, startUtc, cancellationToken);
        var sub = daily * days;
        Campaign? camp = null;
        if (campaignId is not null)
        {
            camp = await db.Campaigns.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == campaignId.Value, cancellationToken);
        }

        var discount = ApplyCampaign(sub, camp);
        return (sub, discount, sub - discount);
    }
}

using AracKiralama.Data.Entities;
using AracKiralama.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AracKiralama.Data;

public static class RentalScheduling
{
    /// <summary>
    /// Taslak, onaylı ve aktif kiralamalar aynı araç için zaman aralığında çakışamaz.
    /// </summary>
    public static async Task<bool> HasOverlapAsync(
        AppDbContext db,
        int vehicleId,
        DateTime startUtc,
        DateTime endUtc,
        int? excludeRentalId,
        CancellationToken cancellationToken = default)
    {
        if (endUtc <= startUtc)
        {
            return true;
        }

        return await db.Rentals
            .AsNoTracking()
            .Where(r => r.VehicleId == vehicleId)
            .Where(r =>
                r.State == RentalState.Draft
                || r.State == RentalState.Confirmed
                || r.State == RentalState.Active)
            .Where(r => excludeRentalId == null || r.Id != excludeRentalId.Value)
            .AnyAsync(r => r.StartUtc < endUtc && r.EndUtc > startUtc, cancellationToken);
    }
}

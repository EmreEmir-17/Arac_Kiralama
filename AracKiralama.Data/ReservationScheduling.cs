using AracKiralama.Data.Entities;
using AracKiralama.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace AracKiralama.Data;

public static class ReservationScheduling
{
    public static async Task<bool> HasOverlapAsync(
        AppDbContext db,
        int vehicleId,
        DateTime startUtc,
        DateTime endUtc,
        int? excludeReservationId,
        CancellationToken cancellationToken = default)
    {
        if (endUtc <= startUtc)
        {
            return true;
        }

        return await db.Reservations
            .AsNoTracking()
            .Where(r => r.VehicleId == vehicleId)
            .Where(r => r.State == ReservationState.Pending || r.State == ReservationState.Confirmed)
            .Where(r => excludeReservationId == null || r.Id != excludeReservationId.Value)
            .AnyAsync(r => r.StartUtc < endUtc && r.EndUtc > startUtc, cancellationToken);
    }
}

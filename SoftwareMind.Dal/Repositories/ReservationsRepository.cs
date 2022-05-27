using Microsoft.EntityFrameworkCore;
using SoftwareMind.Dal.Entities;

namespace SoftwareMind.Dal.Repositories;

public class ReservationsRepository : IReservationsRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ReservationsRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Reservation>> GetReservationsAsync(Guid locationId, Guid deskId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Reservations.Where(r => r.LocationId == locationId && r.DeskId == deskId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Reservation?> GetReservationById(Guid locationId, Guid deskId, Guid id,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Reservations.FirstOrDefaultAsync(
            r => r.LocationId == locationId && r.DeskId == deskId && r.Id == id,
            cancellationToken);
    }

    public async Task<Guid> ReserveDesk(Reservation reservation, CancellationToken cancellationToken)
    {
        await _dbContext.Reservations.AddAsync(reservation, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return reservation.Id;
    }

    public async Task ChangeReservation(Reservation newReservation, Reservation oldReservation,
        CancellationToken cancellationToken)
    {
        var reservation = await GetReservationById(oldReservation.LocationId, oldReservation.DeskId, oldReservation.Id,
            cancellationToken);

        reservation!.LocationId = newReservation.LocationId;
        reservation.DeskId = newReservation.DeskId;
        reservation.StartDate = newReservation.StartDate;
        reservation.EndDate = newReservation.EndDate;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}